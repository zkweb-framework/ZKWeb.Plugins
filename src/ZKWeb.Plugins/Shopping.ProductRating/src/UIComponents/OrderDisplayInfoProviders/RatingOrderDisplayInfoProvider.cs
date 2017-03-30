using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.UIComponents.OrderDisplayInfoProviders {
	/// <summary>
	/// 前台订单添加评价的按钮
	/// </summary>
	[ExportMany]
	public class RatingOrderDisplayInfoProvider : IOrderDisplayInfoProvider {
		/// <summary>
		/// 添加订单操作
		/// </summary>
		public void AddActions(SellerOrder order, IList<HtmlString> actions, OrderOperatorType operatorType) {
			// 买家
			var productRatingManager = Application.Ioc.Resolve<ProductRatingManager>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			if (operatorType == OrderOperatorType.Buyer) {
				// 评价
				var canRate = productRatingManager.CanRateOrder(order);
				if (canRate) {
					actions.Add(DefaultOrderDisplayInfoProvider.GetLinkAction(
						templateManager,
						new T("Rate"),
						productRatingManager.GetRatingUrl(order),
						"fa fa-star-o",
						"btn btn-primary"));
				}
			}
		}

		/// <summary>
		/// 添加工具按钮
		/// </summary>
		public void AddToolButtons(SellerOrder order, IList<HtmlString> toolButtons, OrderOperatorType operatorType) {
		}

		/// <summary>
		/// 添加详细信息
		/// </summary>
		public void AddSubjects(SellerOrder order, IList<HtmlString> subjects, OrderOperatorType operatorType) {
		}

		/// <summary>
		/// 添加警告信息
		/// </summary>
		public void AddWarnings(SellerOrder order, IList<HtmlString> warnings, OrderOperatorType operatorType) {
		}
	}
}
