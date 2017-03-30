using ZKWeb;
using ZKWeb.Logging;

namespace WxPayAPI {
	/// <summary>
	/// 回调处理基类
	/// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
	/// 子类在此类基础上进行派生并重写自己的回调处理过程
	/// </summary>
	public static class WxPayNotify {
		/// <summary>
		/// 接收从微信支付后台发送过来的数据
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="xml">微信发来的xml内容</param>
		/// <returns>微信支付后台返回的数据</returns>
		public static WxPayData GetNotifyData(WxPayConfig config, string xml) {
			// 接收从微信后台POST过来的数据
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug($"WxPay receive notify: {xml}");
			// 转换数据格式并验证签名
			WxPayData data = new WxPayData();
			try {
				data.FromXml(config, xml);
			} catch (WxPayException) {
				// 若签名错误，则立即返回结果给微信支付后台
				var msg = $"WxPay sign check error: {xml}";
				logManager.LogDebug(msg);
				throw new WxPayException(msg);
			}
			logManager.LogDebug($"WxPay check sign success");
			return data;
		}
	}
}