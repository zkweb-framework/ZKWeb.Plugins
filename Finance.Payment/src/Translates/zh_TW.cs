using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "PaymentApi", "支付接口" },
			{ "PaymentTransaction", "支付交易" },
			{ "Support receive money through payment api", "支持通過支付接口進行收款" },
			{ "PaymentApiManage", "支付接口管理" },
			{ "TestPayment", "測試支付" },
			{ "TestApi", "測試接口" },
			{ "Name/Owner/Remark", "名稱/所屬人/備注" },
			{ "PaymentTransactionRecords", "支付交易記錄" },
			{ "PaymentApiType", "支付接口類型" },
			{ "NextStep", "下一步" },
			{ "Payment api type not specificed", "沒有指定支付接口的類型" },
			{ "Owner", "所屬人" },
			{ "Please enter owner's username, keep empty if owned by system", "請填寫所屬人的用戶名，屬於系統請留空" },
			{ "Specificed payment api owner not exist", "指定的支付接口的所屬人不存在" },
			{ "Cannot change type after payment api created", "支付接口創建後不能修改類型" },
			{ "Cannot change owner after payment api created", "支付接口創建後不能修改所屬人" },
			{ "SupportTransactionTypes", "支持的交易類型" },
			{ "TestTransaction", "測試交易" },
			{ "PaymentPassword", "支付密碼" },
			{ "Password required to pay transactions", "支付時要求的密碼" },
			{ "Add transaction from admin panel is not supported", "不能從後台直接添加支付交易" },
			{ "Serial/Payer/Payee/Description/Remark", "流水號/付款人/收款人/描述/備注" },
			{ "Serial", "流水號" },
			{ "ExternalSerial", "外部流水號" },
			{ "ApiName", "接口名稱" },
			{ "Amount", "金額" },
			{ "Payer", "付款人" },
			{ "Payee", "收款人" },
			{ "State", "狀態" },
			{ "Test Payment Api", "測試支付接口" },
			{ "Test payment api is usable", "測試支付接口是否可用" },
			{ "Payment api not exist", "支付接口不存在" },
			{ "Unknown payment transaction type {0}", "未知的支付交易類型{0}" },
			{ "Unknown payment api type {0}", "未知的支付交易接口類型{0}" },
			{ "Transaction amount must large than zero", "交易金額必須大於零" },
			{ "Transaction description is required", "交易描述不能為空" },
			{ "Selected payment api not support this transaction", "選擇的支付接口不支持當前交易" },
			{ "Selected payment api is deleted", "選擇的支付接口已被刪除，不能使用" },
			{ "Transaction Created", "交易已創建" },
			{ "Payment transaction not found", "支付交易不存在" },
			{ "Payment transaction {0} error: {1}", "支付交易{0}錯誤: {1}" },
			{ "Payer of transaction not logged in", "交易的付款人未登陸" },
			{ "Transaction not waiting for pay", "交易不在等待付款中" },
			{ "Transaction not at initial state, can't set to waiting paying", "交易不在初始狀態，無法設置成等待付款" },
			{ "Transaction not waiting for pay or confirm, can't set to success", "交易不在等待付款或確認中，無法設置成交易成功" },
			{ "Transaction already aborted, can't process again", "交易已中止，不能重復處理" },
			{ "Only secured paid transaction can call send goods api", "只有擔保交易已付款的交易可以調用發貨接口" },
			{ "Unsupported transaction state: {0}", "不支持的交易狀態: {0}" },
			{ "InitialState", "初始狀態" },
			{ "WaitingPaying", "等待付款" },
			{ "SecuredPaid", "擔保交易已付款" },
			{ "TransactionSuccess", "交易成功" },
			{ "TransactionAborted", "交易中止" },
			{ "Change transaction state to {0}", "切換交易狀態到{0}" },
			{ "Notify goods sent to payment api success", "通知支付接口已發貨成功" },
			{ "Create test transaction success, redirecting to payment page...", "創建測試交易成功，正在跳轉到支付頁……" },
			{ "Pay", "支付" },
			{ "Use test api to pay transaction created with other api type is not allowed", "不能使用測試接口支付選擇了其他接口的交易" },
			{ "TestApi Pay", "使用測試接口支付" },
			{ "You need provide the password entered at test api creation, after submit transaction will be paid",
				"您需要提供創建測試接口時填寫的密碼，提交後交易會變成已支付" },
			{ "PayType", "支付類型" },
			{ "ImmediateArrival", "即時到賬" },
			{ "SecuredTransaction", "擔保交易" },
			{ "Incorrect payment password", "支付密碼不正確" },
			{ "Redirecting to payment transaction records...", "正在跳轉到交易記錄頁……" },
			{ "Payment Successfully, Redirecting to result page...", "支付成功，正在跳轉到結果頁……" },
			{ "LastError", "最後發生的錯誤" },
			{ "DetailRecords", "詳細記錄" },
			{ "Creator", "創建人" },
			{ "Contents", "內容" },
			{ "PayResult", "支付結果" },
			{ "Test", "測試" },
			{ "PaymentFee", "支付手續費" },
			{ "Selected payment api does not exist", "您選擇的支付接口不存在" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
