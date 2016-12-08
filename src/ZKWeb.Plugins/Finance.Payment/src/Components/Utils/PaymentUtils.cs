using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.Utils {
	/// <summary>
	/// 支付相关的帮助函数
	/// </summary>
	public static class PaymentUtils {
		/// <summary>
		/// 获取返回或异步通知Url，如果自定义了域名则使用自定义的域名
		/// </summary>
		/// <param name="returnDomain">返回域名</param>
		/// <param name="url">Url</param>
		/// <returns></returns>
		public static string GetReturnOrNotifyUrl(string returnDomain, string url) {
			if (string.IsNullOrEmpty(returnDomain)) {
				returnDomain = HttpManager.CurrentContext.Request.Host;
			}
			if (!returnDomain.StartsWith("http://") ||
				returnDomain.StartsWith("https://")) {
				returnDomain = "http://" + returnDomain;
			}
			if (returnDomain.EndsWith("/")) {
				returnDomain = returnDomain.Substring(0, returnDomain.Length - 1);
			}
			if (!url.StartsWith("/")) {
				url = "/" + url;
			}
			return returnDomain + url;
		}
	}
}
