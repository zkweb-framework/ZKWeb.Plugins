using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Extensions;

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

		/// <summary>
		/// 转换到编辑使用的列表
		/// </summary>
		/// <param name="values">数据库中的商品匹配数据列表</param>
		/// <returns></returns>
		public static List<ProductMatchedDataForEdit> ToEditList(this ISet<ProductMatchedData> values) {
			if (values == null) {
				return null;
			}
			return values.OrderBy(v => v.MatchOrder).Select(v => {
				// 部分特殊字段需要手动设置到Affects中
				var data = new ProductMatchedDataForEdit() {
					Conditions = v.Conditions,
					Affects = v.Affects
				};
				data.Affects["Price"] = v.Price;
				data.Affects["PriceCurrency"] = v.PriceCurrency;
				data.Affects["Weight"] = v.Weight;
				data.Affects["Stock"] = v.Stock;
				data.Affects["Remark"] = v.Remark;
				return data;
			}).ToList();
		}

		/// <summary>
		/// 转换到数据库使用的集合
		/// </summary>
		/// <param name="values">编辑后的商品匹配数据列表</param>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static ISet<ProductMatchedData> ToDatabaseSet(
			this List<ProductMatchedDataForEdit> values, Database.Product product) {
			if (values == null) {
				return new HashSet<ProductMatchedData>();
			}
			long matchOrder = 0;
			return new HashSet<ProductMatchedData>(values.Select(v => {
				// 部分特殊字段需要手动设置字段中
				var data = new ProductMatchedData() {
					Product = product,
					Conditions = v.Conditions,
					Affects = v.Affects,
					Price = v.Affects.GetOrDefault<decimal?>("Price"),
					PriceCurrency = v.Affects.GetOrDefault<string>("PriceCurrency"),
					Weight = v.Affects.GetOrDefault<decimal?>("Weight"),
					Stock = v.Affects.GetOrDefault<long?>("Stock"),
					MatchOrder = matchOrder++,
					Remark = v.Affects.GetOrDefault<string>("Remark")
				};
				return data;
			}));
		}
	}
}
