using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchedDataMatchers.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions {
	/// <summary>
	/// 商品匹配数据的扩展函数
	/// </summary>
	public static class ProductMatchedDataExtensions {
		/// <summary>
		/// 返回匹配的商品匹配数据
		/// </summary>
		/// <param name="list">商品匹配数据的列表</param>
		/// <param name="matchParameters">商品匹配参数</param>
		/// <returns></returns>
		public static IEnumerable<ProductMatchedData> WhereMatched(
			this IEnumerable<ProductMatchedData> list, ProductMatchParameters matchParameters) {
			var matchers = Application.Ioc.ResolveMany<IProductMatchedDataMatcher>();
			return list.Where(data => matchers.All(m => m.IsMatched(matchParameters, data)));
		}

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
		/// 减少匹配数据中的库存
		/// 有可能会减到负数但不会失败
		/// </summary>
		/// <param name="matchedData">匹配数据</param>
		/// <param name="delta">减少值</param>
		public static void ReduceStock(this ProductMatchedData matchedData, long delta) {
			matchedData.Stock -= delta;
			matchedData.Affects["Stock"] = matchedData.Stock;
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
				data.Affects["ItemNo"] = v.ItemNo;
				data.Affects["BarCode"] = v.BarCode;
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
			this IList<ProductMatchedDataForEdit> values, Entities.Product product) {
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
					ItemNo = v.Affects.GetOrDefault<string>("ItemNo"),
					BarCode = v.Affects.GetOrDefault<string>("BarCode"),
					MatchOrder = matchOrder++,
					Remark = v.Affects.GetOrDefault<string>("Remark")
				};
				return data;
			}));
		}
	}
}
