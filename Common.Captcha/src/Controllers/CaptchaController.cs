using System;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Captcha.src.Managers;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Captcha.src.Controllers {
	/// <summary>
	/// 验证码控制器
	/// </summary>
	[ExportMany]
	public class CaptchaController : IController {
		/// <summary>
		/// 获取验证码
		/// </summary>
		/// <returns></returns>
		[Action("captcha")]
		public IActionResult Captcha() {
			var request = HttpManager.CurrentContext.Request;
			var key = request.Get<string>("key");
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			return new ImageResult(captchaManager.Generate(key));
		}

		/// <summary>
		/// 获取验证码的语音提示
		/// </summary>
		/// <returns></returns>
		[Action("captcha/audio")]
		public IActionResult CaptchaAudio() {
			var request = HttpManager.CurrentContext.Request;
			var key = request.Get<string>("key");
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			var stream = captchaManager.GetAudioStream(key);
			return new StreamResult(stream, "audio/wav");
		}
	}
}
