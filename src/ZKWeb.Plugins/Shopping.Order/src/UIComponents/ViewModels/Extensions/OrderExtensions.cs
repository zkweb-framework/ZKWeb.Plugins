using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单的扩展函数
	/// </summary>
	public static class OrderExtensions {
		/// <summary>
		/// 转换订单到显示信息
		/// </summary>
		/// <param name="order">订单</param>
		/// <returns></returns>
		public static OrderDisplayInfo ToDisplayInfo(this SellerOrder order) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var info = new OrderDisplayInfo();
			info.Serial = order.Serial;
			info.BuyerId = order.Buyer?.Id;
			info.Buyer = order.Buyer?.Username;
			info.SellerId = order.Owner?.Id;
			info.Seller = order.Owner?.Username;
			info.State = order.State;
			info.StateDescription = new T(order.State.GetDescription());
			info.OrderParameters = order.OrderParameters;
			info.TotalCost = order.TotalCost;
			info.OriginalTotalCost = order.OriginalTotalCostCalcResult.Parts.Sum();
			info.Currency = currencyManager.GetCurrency(order.Currency);
			info.RemarkFlags = order.RemarkFlags;
			info.CreateTime = order.CreateTime.ToClientTimeString();
			info.OrderProducts = order.OrderProducts.Select(p => p.ToDisplayInfo()).ToList();
			return info;
		}
	}
}
