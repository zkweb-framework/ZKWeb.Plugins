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
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders {
	/// <summary>
	/// 默认的订单显示信息提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderDisplayInfoProvider : IOrderDisplayInfoProvider {
		/// <summary>
		/// 获取打开模态框的订单操作
		/// </summary>
		/// <returns></returns>
		public static HtmlString GetModalAction(
			TemplateManager templateManager,
			string name, string url, string iconClass,
			string title = null, string buttonClass = null, object dialogParameters = null) {
			buttonClass = buttonClass ?? "btn btn-primary";
			title = title ?? name;
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_action.btn_modal.html",
				new { name, title, url, iconClass, buttonClass, dialogParameters }));
		}

		/// <summary>
		/// 获取已禁用的订单操作
		/// </summary>
		/// <returns></returns>
		public static HtmlString GetDisabledAction(
			TemplateManager templateManager,
			string name, string message, string iconClass) {
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_action.btn_disabled.html",
				new { name, message, iconClass }));
		}

		/// <summary>
		/// 获取跳转链接的订单操作
		/// </summary>
		/// <returns></returns>
		public static HtmlString GetLinkAction(
			TemplateManager templateManager,
			string name, string url, string iconClass,
			string buttonClass = null, string target = null) {
			buttonClass = buttonClass ?? "btn btn-primary";
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_action.btn_link.html",
				new { name, url, target, iconClass, buttonClass }));
		}

		/// <summary>
		/// 添加订单操作
		/// </summary>
		public void AddActions(
			SellerOrder order, IList<HtmlString> actions, OrderOperatorType operatorType) {
			// 买家
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			if (operatorType == OrderOperatorType.Buyer) {
				// 立刻付款
				var canPay = order.Check(c => c.CanPay);
				if (canPay.First) {
					actions.Add(GetLinkAction(templateManager,
						new T("PayNow"),
						orderManager.GetCheckoutUrl(order.Id), "fa fa-credit-card"));
				}
				// 取消订单
				var canSetCancelled = order.Check(c => c.CanSetCancelled);
				if (canSetCancelled.First) {
					actions.Add(GetModalAction(templateManager,
						new T("CancelOrder"),
						$"/user/orders/cancel_order?serial={order.Serial}",
						"fa fa-exclamation-triangle",
						buttonClass: "btn btn-warning",
						dialogParameters: new { type = "type-warning", size = "size-normal" }));
				}
				// 确认收货
				var canConfirm = order.Check(c => c.CanConfirm);
				if (canConfirm.First) {
					actions.Add(GetLinkAction(templateManager,
						new T("ConfirmOrder"),
						$"/user/orders/confirm_order?serial={order.Serial}",
						"fa fa-check", buttonClass: "btn btn-warning"));
				}
			}
			// 卖家或管理员
			if (operatorType == OrderOperatorType.Seller ||
				operatorType == OrderOperatorType.Admin) {
				// 修改价格
				var canEditCost = order.Check(c => c.CanEditCost);
				if (canEditCost.First) {
					actions.Add(GetModalAction(templateManager,
						new T("EditCost"),
						$"/admin/orders/edit_cost?id={order.Id}", "fa fa-credit-card"));
				} else {
					actions.Add(GetDisabledAction(templateManager,
						new T("EditCost"),
						canEditCost.Second, "fa fa-credit-card"));
				}
				// 修改地址
				var canEditShippingAddress = order.Check(c => c.CanEditShippingAddress);
				if (canEditShippingAddress.First) {
					actions.Add(GetModalAction(templateManager,
						new T("EditShippingAddress"),
						$"/admin/orders/edit_shipping_address?id={order.Id}", "fa fa-location-arrow"));
				} else {
					actions.Add(GetDisabledAction(templateManager,
						new T("EditShippingAddress"),
						canEditShippingAddress.Second, "fa fa-location-arrow"));
				}
				// 发货
				var canSendGoods = order.Check(c => c.CanSendGoods);
				if (canSendGoods.First) {
					actions.Add(GetModalAction(templateManager,
						new T("SendGoods"),
						$"/admin/orders/send_goods?id={order.Id}", "fa fa-truck"));
				} else {
					actions.Add(GetDisabledAction(templateManager,
						new T("SendGoods"),
						canSendGoods.Second, "fa fa-truck"));
				}
			}
			// 管理员
			if (operatorType == OrderOperatorType.Admin) {
				// 代确认收货
				var canConfirm = order.Check(c => c.CanConfirm);
				if (canConfirm.First) {
					actions.Add(GetModalAction(templateManager,
						new T("ConfirmInsteadOfBuyer"),
						$"/admin/orders/confirm_instead_of_buyer?id={order.Id}",
						"fa fa-check", buttonClass: "btn btn-success"));
				} else {
					actions.Add(GetDisabledAction(templateManager,
						new T("ConfirmInsteadOfBuyer"),
						canConfirm.Second, "fa fa-check"));
				}
				// 作废
				var canSetInvalid = order.Check(c => c.CanSetInvalid);
				if (canSetInvalid.First) {
					actions.Add(GetModalAction(templateManager,
						new T("SetInvalid"),
						$"/admin/orders/set_invalid?id={order.Id}",
						"fa fa-exclamation-triangle",
						buttonClass: "btn btn-danger",
						dialogParameters: new { type = "type-danger", size = "size-normal" }));
				} else {
					actions.Add(GetDisabledAction(templateManager,
						new T("SetInvalid"),
						canConfirm.Second, "fa fa-exclamation-triangle"));
				}
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
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactions = orderManager.GetReleatedTransactions(order.Id);
			if (transactions.Any(t => t.State == PaymentTransactionState.SecuredPaid)) {
				if (operatorType == OrderOperatorType.Buyer) {
					warnings.Add(HtmlString.Encode(
						new T("Buyer is using secured paid, please tell the buyer confirm transaction on payment platform after received goods")));
				} else {
					warnings.Add(HtmlString.Encode(
						new T("You're using secured paid, please confirm transaction on payment platform after received goods")));
				}
			}
			// 警告关联交易的最后发生错误
			var lastErrors = transactions.Select(t => t.LastError).Where(e => !string.IsNullOrEmpty(e));
			foreach (var lastError in lastErrors) {
				warnings.Add(HtmlString.Encode(
					string.Format(new T("Releated transaction contains error: {0}"), new T(lastError))));
			}
		}
	}
}
