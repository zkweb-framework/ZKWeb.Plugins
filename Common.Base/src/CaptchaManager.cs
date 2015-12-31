using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 验证码管理器
	/// </summary>
	[ExportMany]
	public class CaptchaManager {
		/// <summary>
		/// 默认的验证码位数
		/// </summary>
		public const int DefaultDigits = 4;
		/// <summary>
		/// 生成验证码后最小保留会话的时间
		/// </summary>
		public const int MakeSessionAliveAtLeast = 30;
		/// <summary>
		/// 保存到会话时使用的键名前缀
		/// </summary>
		public const string SessionItemKeyPrefix = "Common.Base.Captcha.";

		/// <summary>
		/// 生成验证码并储存验证码到会话中
		/// </summary>
		public virtual Image Generate(string key, int digits = DefaultDigits, string chars = null) {
			// 生成验证码
			var captchaCode = RandomUtils.RandomString(digits, "23456789ABCDEFGHJKLMNPQRSTUWXYZ");
			var image = new Bitmap(digits * 20, 32);
			var imageRect = new Rectangle(0, 0, image.Width, image.Height);
			var font = new Font("Arial", 20, FontStyle.Regular);
			var brush = new SolidBrush(Color.Black);
			using (var graphic = Graphics.FromImage(image)) {
				// 描画背景
				HatchStyle backgroundStyle = (HatchStyle)RandomUtils.Generator.Next(18, 52);
				while (!Enum.IsDefined(typeof(HatchStyle), backgroundStyle)) {
					backgroundStyle = (HatchStyle)RandomUtils.Generator.Next(18, 52);
				}
				graphic.FillRectangle(
					new HatchBrush(backgroundStyle, Color.Gray, Color.White), imageRect);
				// 描画文本，然后不规则拉伸
				GraphicsPath path = new GraphicsPath();
				path.AddString(captchaCode, font.FontFamily, (int)font.Style, font.Size, imageRect,
					new StringFormat()
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
				int padding = 5;
				Func<int> randomPadding = () => RandomUtils.Generator.Next(padding);
				var points = new PointF[] {
						new PointF(randomPadding(), randomPadding()),
						new PointF(image.Width - randomPadding(), randomPadding()),
						new PointF(randomPadding(), image.Height - randomPadding()),
						new PointF(image.Width - randomPadding(), image.Height - randomPadding()),
					};
				path.Warp(points, imageRect);
				graphic.FillPath(
					new HatchBrush(HatchStyle.LargeConfetti, Color.Black), path);
			}
			// 储存到会话，会话的过期时间最少是30分钟
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			session.Items[SessionItemKeyPrefix + key] = captchaCode;
			session.SetExpiresAtLeast(TimeSpan.FromMinutes(MakeSessionAliveAtLeast));
			sessionManager.SaveSession();
			// 返回图片对象
			return image;
		}

		/// <summary>
		/// 获取当前验证码，但不删除
		/// 这个函数一般只用于单元测试，检查请使用Check函数
		/// </summary>
		public virtual string GetWithoutRemove(string key) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			return session.Items.GetOrDefault<string>(SessionItemKeyPrefix + key);
		}

		/// <summary>
		/// 检查验证码是否正确，检查后删除原验证码
		/// </summary>
		public virtual bool Check(string key, string actualCode) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var itemKey = SessionItemKeyPrefix + key;
			var exceptedCode = session.Items.GetOrDefault<string>(itemKey);
			session.Items.Remove(itemKey);
			sessionManager.SaveSession();
			return !string.IsNullOrEmpty(exceptedCode) &&
				!string.IsNullOrEmpty(actualCode) &&
				actualCode.ToLower() == exceptedCode.ToLower();
		}
	}
}
