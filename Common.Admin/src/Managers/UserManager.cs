using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Managers {
	/// <summary>
	/// 用户管理器
	/// </summary>
	[ExportMany]
	public class UserManager {
		/// <summary>
		/// 记住登陆时，保留会话的天数
		/// </summary>
		public const int SessionExpireDaysWithRememebrLogin = 30;
		/// <summary>
		/// 不记住登陆时，保留会话的天数
		/// </summary>
		public const int SessionExpireDaysWithoutRememberLogin = 1;
		/// <summary>
		/// 头像宽度
		/// </summary>
		public const int AvatarWidth = 150;
		/// <summary>
		/// 头像高度
		/// </summary>
		public const int AvatarHeight = 150;

		/// <summary>
		/// 注册用户
		/// 注册失败时会抛出例外
		/// </summary>
		public virtual void Reg(
			string username, string password, Action<User> update = null) {
			UnitOfWork.WriteData<User>(repository => {
				var user = new User();
				user.Type = UserTypes.User;
				user.Username = username;
				user.SetPassword(password);
				user.CreateTime = DateTime.UtcNow;
				repository.Save(ref user, update);
			});
		}

		/// <summary>
		/// 根据用户名查找用户
		/// 找不到时返回null
		/// </summary>
		public virtual User FindUser(string username) {
			var callbacks = Application.Ioc.ResolveMany<IUserLoginCallback>();
			User user = null;
			UnitOfWork.Read(context => {
				// 通过回调查找用户
				foreach (var callback in callbacks) {
					user = callback.FindUser(context, username);
					if (user != null) {
						return;
					}
				}
				// 通过用户名查找用户，要求未删除
				user = context.Get<User>(u => u.Username == username && !u.Deleted);
			});
			return user;
		}

		/// <summary>
		/// 登陆用户
		/// 登陆失败时会抛出例外
		/// </summary>
		public virtual void Login(string username, string password, bool rememberLogin) {
			// 用户不存在或密码错误时抛出例外
			var user = FindUser(username);
			if (user == null || !user.CheckPassword(password)) {
				throw new HttpException(401, new T("Incorrect username or password"));
			}
			// 以指定用户登录
			LoginWithUser(user, rememberLogin);
		}

		/// <summary>
		/// 以指定用户登录
		/// 跳过密码等检查
		/// </summary>
		public virtual void LoginWithUser(User user, bool rememberLogin) {
			// 获取回调
			var callbacks = Application.Ioc.ResolveMany<IUserLoginCallback>().ToList();
			// 登陆前的处理
			callbacks.ForEach(c => c.BeforeLogin(user));
			// 设置会话
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession();
			var session = sessionManager.GetSession();
			session.ReGenerateId();
			session.ReleatedId = user.Id;
			session.RememberLogin = rememberLogin;
			session.SetExpiresAtLeast(TimeSpan.FromDays(session.RememberLogin ?
				SessionExpireDaysWithRememebrLogin : SessionExpireDaysWithoutRememberLogin));
			sessionManager.SaveSession();
			// 登陆后的处理
			callbacks.ForEach(c => c.AfterLogin(user));
		}

		/// <summary>
		/// 退出登录
		/// </summary>
		public virtual void Logout() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			sessionManager.RemoveSession();
		}

		/// <summary>
		/// 获取登录后应该跳转到的url
		/// </summary>
		/// <returns></returns>
		public virtual string GetUrlRedirectAfterLogin() {
			var request = HttpContext.Current.Request;
			var referrer = request.UrlReferrer;
			// 来源于同一站点时，跳转到来源页面
			if (referrer != null && referrer.Host == request.Url.Host &&
				referrer.AbsolutePath != "/user/logout") {
				return referrer.PathAndQuery;
			}
			// 默认跳转到首页
			return "/";
		}

		/// <summary>
		/// 获取用户头像的网页图片路径，不存在时返回默认图片路径
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual string GetAvatarWebPath(long userId) {
			if (!File.Exists(GetAvatarStoragePath(userId))) {
				// 没有自定义头像时使用默认头像
				return "/static/common.admin.images/default-avatar.jpg";
			}
			return string.Format("/static/common.admin.images/avatar_{0}.jpg", userId);
		}

		/// <summary>
		/// 获取用户头像的储存路径，文件不一定存在
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual string GetAvatarStoragePath(long userId) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			return pathManager.GetStorageFullPath(
				"static", "common.admin.images", string.Format("avatar_{0}.jpg", userId));
		}

		/// <summary>
		/// 保存头像，返回是否成功和错误信息
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="imageStream">图片数据流</param>
		public virtual void SaveAvatar(long userId, Stream imageStream) {
			if (imageStream == null) {
				throw new HttpException(400, new T("Please select avatar file"));
			}
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new HttpException(400, new T("Parse uploaded image failed"));
			}
			using (image) {
				var path = GetAvatarStoragePath(userId);
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				using (var newImage = image.Resize(
					AvatarWidth, AvatarHeight, ImageResizeMode.Padding, Color.White)) {
					newImage.SaveJpeg(path, 90);
				}
			}
		}

		/// <summary>
		/// 删除头像
		/// </summary>
		/// <param name="userId">用户Id</param>
		public void DeleteAvatar(long userId) {
			var path = GetAvatarStoragePath(userId);
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="oldPassword">原密码</param>
		/// <param name="newPassword">新密码</param>
		public void ChangePassword(long userId, string oldPassword, string newPassword) {
			UnitOfWork.WriteData<User>(repository => {
				var user = repository.GetById(userId);
				if (user == null) {
					throw new HttpException(400, new T("User not found"));
				} else if (!user.CheckPassword(oldPassword)) {
					throw new HttpException(400, new T("Incorrect old password"));
				}
				repository.Save(ref user, u => u.SetPassword(newPassword));
			});
		}
	}
}
