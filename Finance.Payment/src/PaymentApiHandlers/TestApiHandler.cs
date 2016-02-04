using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentApiHandlers {
	/// <summary>
	/// 测试接口的处理器
	/// </summary>
	[ExportMany]
	public class TestApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "TestApi"; } }

		/// <summary>
		/// 绑定表单
		/// </summary>
		public void OnBind(PaymentApiEditForm form, DatabaseContext context, PaymentApi bindFrom) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		public void OnSubmit(PaymentApiEditForm form, DatabaseContext context, PaymentApi saveTo) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取支付Html
		/// </summary>
		public void GetPaymentHtml(PaymentTransaction transaction, ref HtmlString html) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 调用发货接口
		/// </summary>
		public void SendGoods(PaymentTransaction transaction, string logisticsName, string invoiceNo) {
			throw new NotImplementedException();
		}

	}
}
