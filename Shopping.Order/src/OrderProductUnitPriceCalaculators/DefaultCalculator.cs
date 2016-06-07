﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderProductUnitPriceCalaculators {
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 默认的订单商品单价计算器
	/// </summary>
	[ExportMany]
	public class DefaultCalculator : IOrderProductUnitPriceCalculator {
		/// <summary>
		/// 从商品匹配数据获取匹配的价格和货币单位
		/// </summary>
		public void Calculate(
			long? userId, CreateOrderProductParameters parameters, OrderPriceCalcResult result) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var matchers = Application.Ioc.ResolveMany<IProductMatchedDataMatcher>();
			var basePrice = 0M;
			var currency = currencyManager.GetDefaultCurrency();
			UnitOfWork.ReadData<Product>(r => {
				var product = r.GetById(parameters.ProductId);
				if (product == null) {
					throw new HttpException(400, new T("The product you are try to purchase does not exist."));
				}
				foreach (var data in product.MatchedDatas) {
					if (data.Price != null && matchers.All(m => m.IsMatched(parameters.MatchParameters, data))) {
						basePrice = data.Price.Value;
						currency = data.GetCurrency();
						break;
					}
				}
			});
			result.Currency = currency.Type;
			result.Parts.Add(new OrderPriceCalcResult.Part("ProductUnitPrice", basePrice));
		}
	}
}