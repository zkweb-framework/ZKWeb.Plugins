using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
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
			{ "Name/Owner", "名称/所属人" },
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
			{ "Password required to pay transactions", "支付时要求的密码" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
