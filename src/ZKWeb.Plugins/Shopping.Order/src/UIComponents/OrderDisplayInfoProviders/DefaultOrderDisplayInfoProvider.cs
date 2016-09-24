using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders {
	/// <summary>
	/// 默认的订单显示信息提供器
	/// </summary>
	public class DefaultOrderDisplayInfoProvider : IOrderDisplayInfoProvider {
		/// <summary>
		/// 添加订单操作
		/// </summary>
		public void AddActions(
			SellerOrder order, IList<HtmlString> actions, OrderOperatorType operatorType) {
			actions.Add(new HtmlString("<a>xxx</a>"));
			if (operatorType == OrderOperatorType.Buyer) {
				// TODO:
				// 立刻付款

				// 取消订单

				// 确认收货

			} else {
				// 修改价格

				// 修改地址

				// 发货

				// 代确认收货

				// 作废

				actions.Add(new HtmlString("<a>asdasdas</a>"));
			}
		}

		/// <summary>
		/// 添加工具按钮
		/// </summary>
		public void AddToolButtons(
			SellerOrder order, IList<HtmlString> toolButtons, OrderOperatorType operatorType) {
			// 复制收货人地址的按钮
			var address = order.OrderParameters.GetShippingAddress();
			if (!string.IsNullOrEmpty(address.DetailedAddress)) {
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				toolButtons.Add(new HtmlString(templateManager.RenderTemplate(
					"shopping.order/tmpl.order_view.details_copy_address.html",
					new { summary = address.GenerateSummary() })));
			}
		}

		/// <summary>
		/// 添加详细信息
		/// </summary>
		public void AddSubjects(
			SellerOrder order, IList<HtmlString> subjects, OrderOperatorType operatorType) {
			// 收货地址
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var address = order.OrderParameters.GetShippingAddress();
			if (!string.IsNullOrEmpty(address.DetailedAddress)) {
				subjects.Add(new HtmlString(templateManager.RenderTemplate(
					"shopping.order/tmpl.order_view.details_subject_row.html",
					new { name = new T("ShippingAddress"), value = address.GenerateSummary() })));
			}
			// 买家留言和卖家留言
			var lastBuyerComment = order.OrderComments
				.Where(c => c.Side == OrderCommentSide.BuyerComment)
				.OrderBy(c => c.CreateTime)
				.LastOrDefault()?.Contents;
			var lastSellerComment = order.OrderComments
				.Where(c => c.Side == OrderCommentSide.SellerComment)
				.OrderBy(c => c.CreateTime)
				.LastOrDefault()?.Contents;
			subjects.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.details_comment_row.html",
				new { name = new T("BuyerComment"), value = lastBuyerComment })));
			subjects.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.details_comment_row.html",
				new { name = new T("SellerComment"), value = lastSellerComment })));
			// 订单编号
			subjects.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.details_subject_row.html",
				new { name = new T("OrderSerial"), value = order.Serial })));
			// 下单时间
			subjects.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.details_subject_row.html",
				new { name = new T("CreateTime"), value = order.CreateTime.ToClientTimeString() })));
			// 支付接口
			var paymentApiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var paymentApiId = order.OrderParameters.GetPaymentApiId();
			var payment = paymentApiManager.GetWithCache(paymentApiId);
			subjects.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.details_subject_row.html",
				new { name = new T("PaymentApi"), value = new T(payment?.Name) })));
			// 物流配送
			// 这里只显示买家指定的，不显示后台实际发货使用的
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			var logisticsId = order.OrderParameters.GetSellerToLogistics()
				.GetOrDefault(order.Owner?.Id ?? Guid.Empty);
			var logistics = logisticsManager.GetWithCache(logisticsId);
			if (logistics != null) {
				subjects.Add(new HtmlString(templateManager.RenderTemplate(
					"shopping.order/tmpl.order_view.details_subject_row.html",
					new { name = new T("OrderLogistics"), value = new T(logistics?.Name) })));
			}
		}
		/// <summary>
		/// 添加警告信息
		/// </summary>
		public void AddWarnings(
			SellerOrder order, IList<HtmlString> warnings, OrderOperatorType operatorType) {
			// 警告担保交易未确认收款
			// TODO: 换成HtmlString.Encode
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactions = orderManager.GetReleatedTransactions(order.Id);
			if (transactions.Any(t => t.State == PaymentTransactionState.SecuredPaid)) {
				if (operatorType == OrderOperatorType.Buyer) {
					warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
						new T("Buyer is using secured paid, please tell the buyer confirm transaction on payment platform after received goods"))));
				} else {
					warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
						new T("You're using secured paid, please confirm transaction on payment platform after received goods"))));
				}
			}
			// 警告关联交易的最后发生错误
			// TODO: 换成HtmlString.Encode
			var lastErrors = transactions.Select(t => t.LastError).Where(e => !string.IsNullOrEmpty(e));
			foreach (var lastError in lastErrors) {
				warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
					string.Format(new T("Releated transaction contains error: {0}"), new T(lastError)))));
			}
		}
	}
}
