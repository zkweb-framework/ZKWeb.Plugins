using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Database;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
	/// <summary>
	/// 商品匹配数据的扩展函数
	/// </summary>
	public static class ProductMatchedDataExtensions {
		/// <summary>
		/// 获取货币信息
		/// 没有设置时返回默认货币信息
		/// </summary>
		/// <param name="data">商品匹配数据</param>
		/// <returns></returns>
		public static ICurrency GetCurrency(this ProductMatchedData data) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			return currencyManager.GetCurrency(data.PriceCurrency) ?? currencyManager.GetDefaultCurrency();
		}

		/// <summary>
		/// 从商品匹配数据列表获取价格字符串
		/// 最小价格等于最大价格时返回 最小价格
		/// 不等于时返回 最小价格~最大价格
		/// </summary>
		/// <param name="matchedDatas">商品匹配数据列表</param>
		/// <returns></returns>
		public static string GetPriceString(
			this IEnumerable<ProductMatchedData> matchedDatas) {
			matchedDatas = matchedDatas.Where(d => d.Price != null).OrderBy(d => d.Price);
			if (!matchedDatas.Any()) {
				return null;
			}
			var minPriceData = matchedDatas.First();
			var maxPriceData = matchedDatas.Last();
			var minPrice = minPriceData.GetCurrency().Format(minPriceData.Price.Value);
			var maxPrice = maxPriceData.GetCurrency().Format(maxPriceData.Price.Value);
			return (minPrice == maxPrice) ? minPrice : string.Format("{0}~{1}", minPrice, maxPrice);
		}

		/// <summary>
		/// 从商品匹配数据列表获取总库存字符串
		/// 返回所有数据的库存合计
		/// </summary>
		/// <param name="matchedDatas">商品匹配数据列表</param>
		/// <returns></returns>
		public static string GetTotalStockString(
			this IEnumerable<ProductMatchedData> matchedDatas) {
			return matchedDatas.Select(d => d.Stock)
				.Where(s => s != null)
				.Select(s => s.Value).Sum().ToString();
		}
	}
}
