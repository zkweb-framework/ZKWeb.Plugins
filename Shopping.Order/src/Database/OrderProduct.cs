using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 订单商品
	/// </summary>
	public class OrderProduct {
		/// <summary>
		/// 订单商品Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属的订单
		/// </summary>
		public virtual Order Order { get; set; }
		/// <summary>
		/// 对应的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// 包含规格等信息，这里会包含购买数量但不应该使用这里的数量
		/// </summary>
		public virtual Dictionary<string, object> MatchParameters { get; set; }
		/// <summary>
		/// 购买数量
		/// </summary>
		public virtual long Count { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public virtual decimal UnitPrice { get; set; }
		/// <summary>
		/// 单价的货币
		/// </summary>
		public virtual string Currency { get; set; }
		/// <summary>
		/// 单价的计算结果
		/// </summary>
		public virtual OrderPriceCalcResult UnitPriceCalcResult { get; set; }
		/// <summary>
		/// 原始单价的计算结果
		/// 下单时生成且之后不会改变
		/// </summary>
		public virtual OrderPriceCalcResult OriginalUnitPriceCalcResult { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 关联的属性值集合
		/// </summary>
		public virtual ISet<OrderProductToPropertyValue> PropertyValues { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderProduct() {
			MatchParameters = new Dictionary<string, object>();
			PropertyValues = new HashSet<OrderProductToPropertyValue>();
		}
	}

	/// <summary>
	/// 订单商品的数据库结构
	/// </summary>
	[ExportMany]
	public class OrderProductMap : ClassMap<OrderProduct> {
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderProductMap() {
			Id(p => p.Id);
			References(p => p.Order).Not.Nullable();
			References(p => p.Product).Not.Nullable();
			Map(p => p.MatchParameters).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(p => p.Count);
			Map(p => p.UnitPrice);
			Map(p => p.Currency).Not.Nullable();
			Map(p => p.UnitPriceCalcResult).CustomType<JsonSerializedType<OrderPriceCalcResult>>();
			Map(p => p.OriginalUnitPriceCalcResult).CustomType<JsonSerializedType<OrderPriceCalcResult>>();
			Map(p => p.CreateTime);
			Map(p => p.LastUpdated);
			HasMany(p => p.PropertyValues).Cascade.AllDeleteOrphan();
		}
	}
}
