using System;

namespace WxPayAPI {
	/// <summary>
	/// 微信支付抛出的例外
	/// </summary>
	public class WxPayException : Exception {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="msg">消息</param>
		public WxPayException(string msg) : base(msg) {
		}
	}
}