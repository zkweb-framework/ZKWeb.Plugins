using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "PaymentApi", "支付接口" },
			{ "Support receive money through payment api", "支持通过支付接口进行收款" },
			{ "PaymentApiManage", "支付接口管理" },
			{ "TestPayment", "测试支付" },
			{ "TestApi", "测试接口" },
			{ "Name/Owner/Remark", "名称/所属人/备注" },
			{ "PaymentTransactionRecords", "支付交易记录" },
			{ "PaymentApiType", "支付接口类型" },
			{ "NextStep", "下一步" },
			{ "Payment api type not specificed", "没有指定支付接口的类型" },
			{ "Owner", "所属人" },
			{ "Please enter owner's username, keep empty if owned by system", "请填写所属人的用户名，属于系统请留空" },
			{ "Specificed payment api owner not exist", "指定的支付接口的所属人不存在" },
			{ "Cannot change type after payment api created", "支付接口创建后不能修改类型" },
			{ "Cannot change owner after payment api created", "支付接口创建后不能修改所属人" },
			{ "SupportTransactionTypes", "支持的交易类型" },
			{ "TestTransaction", "测试交易" },
			{ "PaymentPassword", "支付密码" },
			{ "Password required to pay transactions", "支付时要求的密码" },
			{ "Add transaction from admin panel is not supported", "不能从后台直接添加支付交易" },
			{ "Serial/Payer/Payee/Description/Remark", "流水号/付款人/收款人/描述/备注" },
			{ "Serial", "流水号" },
			{ "ExternalSerial", "外部流水号" },
			{ "ApiName", "接口名称" },
			{ "Amount", "金额" },
			{ "Payer", "付款人" },
			{ "Payee", "收款人" },
			{ "State", "状态" },
			{ "Test Payment Api", "测试支付接口" },
			{ "Test payment api is usable", "测试支付接口是否可用" },
			{ "Payment api not exist", "支付接口不存在" },
			{ "Unknown transaction type {0}", "未知的交易类型" },
			{ "Transaction amount must large than zero", "交易金额必须大于零" },
			{ "Transaction description is required", "交易描述不能为空" },
			{ "Selected payment api not support this transaction", "选择的支付接口不支持当前交易" },
			{ "Selected payment api is deleted", "选择的支付接口已被删除，不能使用" },
			{ "Transaction Created", "交易已创建" },
			{ "Payment transaction not found", "支付交易不存在" },
			{ "Payment transaction {0} error: {1}", "支付交易{0}错误: {1}" },
			{ "Payer of transaction not logged in", "交易的付款人未登陆" },
			{ "Transaction not waiting for pay", "交易不在等待付款中" },
			{ "Transaction not at initial state, can't set to waiting paying", "交易不在初始状态，无法设置成等待付款" },
			{ "Transaction not waiting for pay or confirm, can't set to success", "交易不在等待付款或确认中，无法设置成交易成功" },
			{ "Transaction already aborted, can't process again", "交易已中止，不能重复处理" },
			{ "Only secured paid transaction can call send goods api", "只有担保交易已付款的交易可以调用发货接口" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
