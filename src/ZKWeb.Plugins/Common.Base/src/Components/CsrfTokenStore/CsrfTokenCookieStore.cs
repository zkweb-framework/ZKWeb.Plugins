using ZKWeb.Plugins.Common.Base.src.Components.CsrfTokenStore.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Components.CsrfTokenStore {
	/// <summary>
	/// 使用Cookie获取和保存CSRF校验值
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CsrfTokenCookieStore : ICsrfTokenStore {
		/// <summary>
		/// 储存CSRF校验到Cookie时使用的键
		/// </summary>
		public const string CsrfTokenCookieKey = "ZKWEBCSRFTOKEN";

		/// <summary>
		/// 获取CSRF校验
		/// </summary>
		public string GetCsrfToken() {
			var context = HttpManager.CurrentContext;
			return context.GetCookie(CsrfTokenCookieKey);
		}

		/// <summary>
		/// 设置CSRF校验
		/// </summary>
		public void SetCsrfToken(string token) {
			var options = new HttpCookieOptions() { Expires = null, HttpOnly = true };
			var context = HttpManager.CurrentContext;
			context.PutCookie(CsrfTokenCookieKey, token, options);
		}

		/// <summary>
		/// 删除CSRF校验
		/// </summary>
		public void RemoveCsrfToken() {
			var context = HttpManager.CurrentContext;
			context.RemoveCookie(CsrfTokenCookieKey);
		}
	}
}
