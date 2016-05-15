using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding {
	/// <summary>
	/// 图片浏览器的构建器
	/// 例：
	///	TODO: 待补充
	/// </summary>
	public abstract class ImageBrowserBuilder : IPrivilegesProvider, IWebsiteStartHandler {
		/// <summary>
		/// 图片类别
		/// </summary>
		public abstract string Category { get; }
		public string CategoryLower { get { return Category.ToLower(); } }
		/// <summary>
		/// 基础Url
		/// </summary>
		public virtual string Url { get { return "/image_browser/" + CategoryLower; } }
		/// <summary>
		/// 搜索图片的Url
		/// </summary>
		public virtual string SearchUrl { get { return Url + "/search"; } }
		/// <summary>
		/// 上传图片的Url
		/// </summary>
		public virtual string UploadUrl { get { return Url + "/upload"; } }
		/// <summary>
		/// 删除图片的Url
		/// </summary>
		public virtual string RemoveUrl { get { return Url + "/remove"; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public virtual string TemplatePath { get { return "cms.imagebrowser/image_list.html"; } }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public virtual UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 默认需要按类别生成的权限
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return new[] { "ImageManage:" + Category }; } }

		/// <summary>
		/// 浏览图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult Action() {
			return new TemplateResult(TemplatePath);
		}

		/// <summary>
		/// 搜索图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult SearchAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 上传图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult UploadAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 删除图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult RemoveAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<string> GetPrivileges() {
			foreach (var privilege in RequiredPrivileges) {
				yield return privilege;
			}
		}

		/// <summary>
		/// 网站启动时的处理
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
			controllerManager.RegisterAction(UploadUrl, HttpMethods.POST, UploadAction);
			controllerManager.RegisterAction(RemoveUrl, HttpMethods.POST, RemoveAction);
		}
	}
}
