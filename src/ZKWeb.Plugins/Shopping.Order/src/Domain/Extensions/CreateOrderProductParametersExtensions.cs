using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 订单商品创建参数的扩展函数
	/// </summary>
	public static class CreateOrderProductParametersExtensions {
		/// <summary>
		/// 获取包含了实体商品的卖家Id列表
		/// 无卖家的Id等于Guid.Empty
		/// </summary>
		/// <param name="parametersList">订单商品创建参数</param>
		/// <returns></returns>
		public static IList<Guid> GetSellerIdsHasRealProducts(
			this IList<CreateOrderProductParameters> parametersList) {
			var productManager = Application.Ioc.Resolve<ProductManager>();
			return parametersList
				.Select(p => productManager.GetWithCache(p.ProductId))
				.Where(p => p.GetProductType() is IAmRealProduct)
				.Select(p => p.Seller?.Id ?? Guid.Empty)
				.Distinct().ToList();
		}
	}
}
