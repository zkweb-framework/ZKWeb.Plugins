using System;
using ZKWeb;
using ZKWeb.Logging;
using ZKWebStandard.Utils;

namespace WxPayAPI {
	/// <summary>
	/// 微信支付Api包装类
	/// </summary>
	public class WxPayApi {
		/// <summary>
		/// 提交被扫支付API
		/// 收银员使用扫码设备读取微信用户刷卡授权码以后，二维码或条码信息传送至商户收银台，
		/// 由商户收银台或者商户后台调用该接口发起支付。
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给被扫支付API的参数</param>
		/// <param name="timeOut">超时时间</param>
		/// <returns></returns>
		public static WxPayData Micropay(
			WxPayConfig config, WxPayData inputObj, int timeOut = 10) {
			string url = "https://api.mch.weixin.qq.com/pay/micropay";
			// 检测必填参数
			if (!inputObj.IsSet("body")) {
				throw new WxPayException("提交被扫支付API接口中，缺少必填参数body！");
			} else if (!inputObj.IsSet("out_trade_no")) {
				throw new WxPayException("提交被扫支付API接口中，缺少必填参数out_trade_no！");
			} else if (!inputObj.IsSet("total_fee")) {
				throw new WxPayException("提交被扫支付API接口中，缺少必填参数total_fee！");
			} else if (!inputObj.IsSet("auth_code")) {
				throw new WxPayException("提交被扫支付API接口中，缺少必填参数auth_code！");
			}

			inputObj.SetValue("spbill_create_ip", config.IP); // 终端ip
			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay Micropay request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay Micropay response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给查询订单API的参数</param>
		/// <param name="timeOut">超时时间</param>
		/// <returns></returns>
		public static WxPayData OrderQuery(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/pay/orderquery";
			// 检测必填参数
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id")) {
				throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay OrderQuery request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay OrderQuery response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 撤销订单API接口
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个</param>
		/// <param name="timeOut">接口超时时间</param>
		/// <returns></returns>
		public static WxPayData Reverse(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
			// 检测必填参数
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id")) {
				throw new WxPayException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay Reverse request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, true, timeOut);
			logManager.LogDebug("WxPay Reverse response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 申请退款
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给申请退款API的参数</param>
		/// <param name="timeOut">超时时间</param>
		/// <returns></returns>
		public static WxPayData Refund(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
			// 检测必填参数
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id")) {
				throw new WxPayException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
			} else if (!inputObj.IsSet("out_refund_no")) {
				throw new WxPayException("退款申请接口中，缺少必填参数out_refund_no！");
			} else if (!inputObj.IsSet("total_fee")) {
				throw new WxPayException("退款申请接口中，缺少必填参数total_fee！");
			} else if (!inputObj.IsSet("refund_fee")) {
				throw new WxPayException("退款申请接口中，缺少必填参数refund_fee！");
			} else if (!inputObj.IsSet("op_user_id")) {
				throw new WxPayException("退款申请接口中，缺少必填参数op_user_id！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay Refund request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, true, timeOut);
			logManager.LogDebug("WxPay Refund response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 查询退款
		/// 提交退款申请后，通过该接口查询退款状态。退款有一定延时
		/// 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态
		/// out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
		/// </summary>
		/// <param name="config">微信支付接口</param>
		/// <param name="inputObj">提交给查询退款API的参数</param>
		/// <param name="timeOut">接口超时时间</param>
		/// <returns></returns>
		public static WxPayData RefundQuery(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/pay/refundquery";
			// 检测必填参数
			if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
				!inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id")) {
				throw new WxPayException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay RefundQuery request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay RefundQuery response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 下载对账单
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给下载对账单API的参数</param>
		/// <param name="timeOut">接口超时时间</param>
		/// <returns></returns>
		public static WxPayData DownloadBill(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
			// 检测必填参数
			if (!inputObj.IsSet("bill_date")) {
				throw new WxPayException("对账单接口中，缺少必填参数bill_date！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay DownloadBill request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay DownloadBill response: " + response);

			WxPayData result = new WxPayData();
			// 若接口调用失败会返回xml格式的结果
			// 接口调用成功则返回非xml格式的数据
			if (response.Substring(0, 5) == "<xml>") {
				result.FromXml(config, response);
			} else {
				result.SetValue("result", response);
			}
			return result;
		}

		/// <summary>
		/// 转换短链接
		/// 该接口主要用于扫码原生支付模式一中的二维码链接转成短链接(weixin://wxpay/s/XXXXXX)，
		/// 减小二维码数据量，提升扫描速度和精确度。
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给转换短连接API的参数</param>
		/// <param name="timeOut">接口超时时间</param>
		/// <returns></returns>
		public static WxPayData ShortUrl(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/tools/shorturl";
			// 检测必填参数
			if (!inputObj.IsSet("long_url")) {
				throw new WxPayException("需要转换的URL，签名用原串，传输需URL encode！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay ShortUrl request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay ShortUrl response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 统一下单
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给统一下单API的参数</param>
		/// <param name="timeOut">超时时间</param>
		/// <returns></returns>
		public static WxPayData UnifiedOrder(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
			// 检测必填参数
			if (!inputObj.IsSet("out_trade_no")) {
				throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
			} else if (!inputObj.IsSet("body")) {
				throw new WxPayException("缺少统一支付接口必填参数body！");
			} else if (!inputObj.IsSet("total_fee")) {
				throw new WxPayException("缺少统一支付接口必填参数total_fee！");
			} else if (!inputObj.IsSet("trade_type")) {
				throw new WxPayException("缺少统一支付接口必填参数trade_type！");
			}

			// 关联参数
			if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid")) {
				throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
			}
			if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id")) {
				throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
			}

			// 异步通知url未设置，则使用配置文件中的url
			if (!inputObj.IsSet("notify_url")) {
				inputObj.SetValue("notify_url", config.NOTIFY_URL); // 异步通知url
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("spbill_create_ip", config.IP); // 终端ip
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay UnfiedOrder request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay UnfiedOrder response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 关闭订单
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <param name="inputObj">提交给关闭订单API的参数</param>
		/// <param name="timeOut">接口超时时间</param>
		/// <returns></returns>
		public static WxPayData CloseOrder(
			WxPayConfig config, WxPayData inputObj, int timeOut = 6) {
			string url = "https://api.mch.weixin.qq.com/pay/closeorder";
			// 检测必填参数
			if (!inputObj.IsSet("out_trade_no")) {
				throw new WxPayException("关闭订单接口中，out_trade_no必填！");
			}

			inputObj.SetValue("appid", config.APPID); // 公众账号ID
			inputObj.SetValue("mch_id", config.MCHID); // 商户号
			inputObj.SetValue("nonce_str", GenerateNonceStr()); // 随机字符串
			inputObj.SetValue("sign", inputObj.MakeSign(config)); // 签名
			string xml = inputObj.ToXml();

			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogDebug("WxPay CloseOrder request: " + xml);
			string response = WxPayHttpService.Post(config, xml, url, false, timeOut);
			logManager.LogDebug("WxPay CloseOrder response: " + response);

			// 将xml格式的结果转换为对象以返回
			var result = new WxPayData();
			result.FromXml(config, response);
			return result;
		}

		/// <summary>
		/// 根据当前系统时间加随机序列来生成订单号
		/// </summary>
		/// <param name="config">微信支付设置</param>
		/// <returns></returns>
		public static string GenerateOutTradeNo(WxPayConfig config) {
			return string.Format("{0}{1}{2}",
				config.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), RandomUtils.RandomInt(100, 999));
		}

		/// <summary>
		/// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
		/// </summary>
		/// <returns></returns>
		public static string GenerateTimeStamp() {
			var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}

		/// <summary>
		/// 生成随机串，随机串包含字母或数字
		/// </summary>
		/// <returns></returns>
		public static string GenerateNonceStr() {
			return Guid.NewGuid().ToString().Replace("-", "");
		}
	}
}