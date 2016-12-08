using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.ListItemProviders {
	/// <summary>
	/// Ping++的支付渠道提供器
	/// </summary>
	public class PaymentChannelListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取支付渠道列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			// 支付宝
			yield return new ListItem(new T("AlipayAppChannel"), "alipay");
			yield return new ListItem(new T("AlipayWapChannel"), "alipay_wap");
			yield return new ListItem(new T("AlipayPcDirectChannel"), "alipay_pc_direct");
			yield return new ListItem(new T("AlipayQRChannel"), "alipay_qr");
			// 百度钱包
			yield return new ListItem(new T("BaiduPayChannel"), "bfb");
			yield return new ListItem(new T("BaiduPayWapChannel"), "bfb_wap");
			// 银联支付
			yield return new ListItem(new T("UnionPayB2BChannel"), "cp_b2b");
			yield return new ListItem(new T("UnionPayChannel"), "upacp");
			yield return new ListItem(new T("UnionPayWapChannel"), "upacp_wap");
			yield return new ListItem(new T("UnionPayPcChannel"), "upacp_pc");
			// 微信支付
			yield return new ListItem(new T("WeChatAppChannel"), "wx");
			yield return new ListItem(new T("WeChatPubChannel"), "wx_pub");
			yield return new ListItem(new T("WeChatPubQRChannel"), "wx_pub_qr");
			yield return new ListItem(new T("WeChatWapChannel"), "wx_wap");
			// 易宝支付 (需要过多参数，目前难以对接)
			// yield return new ListItem(new T("YeepayWapChannel"), "yeepay_wap");
			// 京东支付
			yield return new ListItem(new T("JingDongPayWapChannel"), "jdpay_wap");
			// 分期乐支付
			yield return new ListItem(new T("FenQiLePayWapChannel"), "fqlpay_wap");
			// 量化派支付
			yield return new ListItem(new T("QuantGroupPayWapChannel"), "qgbc_wap");
			// 招行一网通 (需要另外获取终端用户信息，目前难以对接)
			// yield return new ListItem(new T("CMBWalletChannel"), "cmb_wallet");
			// 苹果支付
			yield return new ListItem(new T("ApplePayChannel"), "applepay_upacp");
			// QQ钱包支付
			yield return new ListItem(new T("QQPayChannel"), "qpay");
		}
	}
}
