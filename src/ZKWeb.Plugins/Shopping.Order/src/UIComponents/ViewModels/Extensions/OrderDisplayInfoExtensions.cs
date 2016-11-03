using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单显示信息的扩展函数
	/// </summary>
	public static class OrderDisplayInfoExtensions {
		/// <summary>
		/// 获取订单列表页的表格头部
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTableHeadingHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.table_heading.html", new { info }));
		}

		/// <summary>
		/// 获取订单总价的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTotalCostHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.total_cost.html", new { info }));
		}

		/// <summary>
		/// 获取订单总价和价格组成部分的Html
		/// 会同时显示订单总价和价格组成部分
		/// 但组成部分中的商品总价会忽略显示
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTotalCostWithPartsHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.total_cost_with_parts.html", new { info }));
		}

		/// <summary>
		/// 获取订单基本信息的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetBaseInformationHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.tab_base_information.html", new { info }));
		}

		/// <summary>
		/// 获取订单留言的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderCommentsHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.tab_comments.html", new { info }));
		}

		/// <summary>
		/// 获取订单操作在表格单元格(订单列表页)中的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderActionsTableCellHtml(this OrderDisplayInfo info) {
			var result = new StringBuilder();
			foreach (var action in info.ActionHtmls) {
				var actionHtml = action.ToString();
				if (actionHtml.Contains("btn-danger") || actionHtml.Contains("action-disabled")) {
					continue;
				}
				result.AppendLine(actionHtml);
			}
			return new HtmlString(result.ToString());
		}

		/// <summary>
		/// 获取发货记录的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <param name="deliveries">发货记录</param>
		/// <returns></returns>
		public static HtmlString GetDeliveryRecordsHtml(
			this OrderDisplayInfo info, IEnumerable<OrderDelivery> deliveries) {
			var table = new StaticTableBuilder();
			table.Columns.Add("Serial");
			table.Columns.Add("OrderLogistics", "150");
			table.Columns.Add("LogisticsSerial", "150");
			table.Columns.Add("DeliveryOperator", "150");
			table.Columns.Add("DeliveryTime", "150");
			table.Columns.Add("Actions", "150");
			deliveries = deliveries.OrderByDescending(d => d.CreateTime);
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			foreach (var delivery in deliveries) {
				var viewUrl = string.Format(info.ViewDeliveryUrlFormat, delivery.Id);
				var action = string.IsNullOrEmpty(viewUrl) ? new HtmlString("") :
					DefaultOrderDisplayInfoProvider.GetModalAction(templateManager,
						new T("View"), viewUrl, "fa fa-edit",
						new T("View Delivery"), "btn btn-xs btn-info");
				table.Rows.Add(new {
					Serial = delivery.Serial,
					OrderLogistics = delivery.Logistics?.Name,
					LogisticsSerial = delivery.LogisticsSerial,
					DeliveryOperator = delivery.Operator?.Username,
					DeliveryTime = delivery.CreateTime.ToClientTimeString(),
					Action = action
				});
			}
			return (HtmlString)table.ToLiquid();
		}

		/// <summary>
		/// 获取订单详细记录的html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderRecordsHtml(this OrderDisplayInfo info) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var table = new StaticTableBuilder();
			table.Columns.Add("CreateTime", "150");
			table.Columns.Add("Creator", "150");
			table.Columns.Add("Contents");
			var records = orderManager.GetDetailRecords(info.Id);
			foreach (var record in records) {
				table.Rows.Add(new {
					Time = record.CreateTime.ToClientTimeString(),
					Creator = record.Creator?.Username,
					Contents = record.Content
				});
			}
			return (HtmlString)table.ToLiquid();
		}

		/// <summary>
		/// 获取相关交易的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetReleatedTransactionsHtml(this OrderDisplayInfo info) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var table = new StaticTableBuilder();
			table.Columns.Add("Serial");
			table.Columns.Add("PaymentApi", "150");
			table.Columns.Add("ExternalSerial", "150");
			table.Columns.Add("Amount", "150");
			table.Columns.Add("State", "150");
			table.Columns.Add("Actions", "150");
			var transactions = orderManager.GetReleatedTransactions(info.Id);
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			foreach (var transaction in transactions) {
				var currency = currencyManager.GetCurrency(transaction.CurrencyType);
				var viewUrl = string.Format(info.ViewTransactionUrlFormat, transaction.Id);
				var action = string.IsNullOrEmpty(viewUrl) ? new HtmlString("") :
					DefaultOrderDisplayInfoProvider.GetModalAction(templateManager,
						new T("View"), viewUrl, "fa fa-edit",
						new T("View Transaction"), "btn btn-xs btn-info");
				table.Rows.Add(new {
					Serial = transaction.Serial,
					PaymentApi = transaction.Api?.Name,
					ExternalSerial = transaction.ExternalSerial,
					Amount = currency.Format(transaction.Amount),
					State = new T(transaction.State.GetDescription()),
					Actions = action
				});
			}
			return (HtmlString)table.ToLiquid();
		}

		/// <summary>
		/// 获取订单商品编辑表格的Html
		/// </summary>
		public static HtmlString GetOrderProductPriceEditHtml(this OrderDisplayInfo info) {
			var table = new StaticTableBuilder();
			table.TableClass += " order-product-price-edit-table";
			table.Columns.Add("Product");
			table.Columns.Add("ProductUnitPrice", "110");
			table.Columns.Add("Quantity", "110");
			foreach (var product in info.OrderProducts) {
				table.Rows.Add(new {
					Product = product.GetSummaryHtml(),
					ProductUnitPrice = product.GetPriceEditor(),
					Quantity = product.GetCountEditor()
				});
			}
			return (HtmlString)table.ToLiquid();
		}

		/// <summary>
		/// 获取订单各项价格编辑表格的Html
		/// </summary>
		public static HtmlString GetOrderCostEditHtml(this OrderDisplayInfo info) {
			var table = new StaticTableBuilder();
			table.TableClass += " order-cost-edit-table";
			table.Columns.Add("OrderPricePart");
			table.Columns.Add("Cost", "110");
			foreach (var part in info.TotalCostCalcResult.Parts) {
				table.Rows.Add(new {
					OrderPricePartType = part.Type,
					OrderPricePart = new T(part.Type),
					Cost = part.GetDeltaEditor()
				});
			}
			return (HtmlString)table.ToLiquid();
		}

		/// <summary>
		/// 获取订单交易金额编辑表格的Html
		/// </summary>
		public static HtmlString GetOrderTransactionEditHtml(this OrderDisplayInfo info) {
			var table = new StaticTableBuilder();
			table.TableClass += " transaction-amount-edit-table";
			table.Columns.Add("PaymentTransaction");
			table.Columns.Add("Amount", "110");
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactions = orderManager.GetReleatedTransactions(info.Id);
			foreach (var transaction in transactions) {
				table.Rows.Add(new {
					PaymentTransactionId = transaction.Id,
					PaymentTransaction = transaction.Serial,
					Amount = transaction.GetAmountEditor()
				});
			}
			return (HtmlString)table.ToLiquid();
		}
	}
}
