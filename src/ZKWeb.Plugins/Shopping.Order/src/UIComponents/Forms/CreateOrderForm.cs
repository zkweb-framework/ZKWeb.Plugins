using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 用于创建订单的表单
	/// </summary>
	[Form("CreateOrderForm", "/api/order/create", SubmitButtonText = "SubmitOrder")]
	public class CreateOrderForm : ModelFormBuilder {
		/// <summary>
		/// 创建订单的参数，格式是Json
		/// </summary>
		[Required]
		[HiddenField("CreateOrderParameters")]
		public string CreateOrderParameters { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var createOrderParameters = string.IsNullOrEmpty(CreateOrderParameters) ?
				new CreateOrderParameters() :
				JsonConvert.DeserializeObject<CreateOrderParameters>(CreateOrderParameters);
			createOrderParameters.SetLoginInfo();
			var result = orderManager.CreateOrder(createOrderParameters);
			var transaction = result.CreatedTransactions.Last();
			var resultUrl = transactionManager.GetResultUrl(transaction.Id);
			return new { script = BaseScriptStrings.Redirect(resultUrl) };
		}
	}
}
