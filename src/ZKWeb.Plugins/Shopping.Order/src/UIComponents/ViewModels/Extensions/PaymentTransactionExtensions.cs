using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 交易的扩展函数
	/// </summary>
	public static class PaymentTransactionExtensions {
		/// <summary>
		/// 获取交易金额的编辑器
		/// </summary>
		public static HtmlString GetAmountEditor(this PaymentTransaction transaction) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.number_input_decimal.html",
				new { extraClass = "transaction-amount", value = transaction.Amount });
			return new HtmlString(html);
		}
	}
}
