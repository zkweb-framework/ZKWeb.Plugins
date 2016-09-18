using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderWarningProviders.Interfaces {
	/// <summary>
	/// 订单警告信息的提供器接口
	/// </summary>
	public interface IOrderWarningProvider {
		/// <summary>
		/// 添加管理员显示的警告信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="warnings">警告信息列表</param>
		void AddWarningsForAdmin(SellerOrder order, IList<string> warnings);

		/// <summary>
		/// 添加卖家显示的警告信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="warnings">警告信息列表</param>
		void AddWarningsForSeller(SellerOrder order, IList<string> warnings);

		/// <summary>
		/// 添加买家显示的警告信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="warnings">警告信息列表</param>
		void AddWarningsForBuyer(SellerOrder order, IList<string> warnings);
	}
}
