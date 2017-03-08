using ZKWebStandard.Web;
using ZKWeb;
using ZKWeb.Logging;
using System.IO;

namespace WxPayAPI {
	/// <summary>
	/// 回调处理基类
	/// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
	/// 子类在此类基础上进行派生并重写自己的回调处理过程
	/// </summary>
	public class WxPayNotify {
		/// <summary>
		/// 接收从微信支付后台发送过来的数据并验证签名
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="context">Http上下文</param>
		/// <returns>微信支付后台返回的数据</returns>
		public WxPayData GetNotifyData(WxPayConfig config, IHttpContext context) {
			// 接收从微信后台POST过来的数据
			string xml;
			using (var reader = new StreamReader(context.Request.Body)) {
				xml = reader.ReadToEnd();
			}
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug($"WxPay receive notify: {xml}");

			// 转换数据格式并验证签名
			WxPayData data = new WxPayData();
			try {
				data.FromXml(config, xml);
			} catch (WxPayException ex) {
				// 若签名错误，则立即返回结果给微信支付后台
				WxPayData res = new WxPayData();
				res.SetValue("return_code", "FAIL");
				res.SetValue("return_msg", ex.Message);
				logManager.LogDebug("WxPay sign check error: " + res.ToXml());
				using (var writer = new StreamWriter(context.Response.Body)) {
					writer.Write(res.ToXml());
				}
				context.Response.End();
			}
			logManager.LogDebug($"WxPay check sign success");
			return data;
		}
	}
}