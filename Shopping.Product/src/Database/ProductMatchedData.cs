using FluentNHibernate.Mapping;
using System.Collections.Generic;
using ZKWeb.Database.UserTypes;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品匹配数据
	/// </summary>
	public class ProductMatchedData {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 属于的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 匹配条件
		/// </summary>
		public virtual Dictionary<string, object> Conditions { get; set; }
		/// <summary>
		/// 影响数据
		/// </summary>
		public virtual Dictionary<string, object> Affects { get; set; }
		/// <summary>
		/// 价格，等于null时继续匹配下一项
		/// </summary>
		public virtual decimal? Price { get; set; }
		/// <summary>
		/// 价格的货币，跟随价格匹配
		/// </summary>
		public virtual string PriceCurrency { get; set; }
		/// <summary>
		/// 重量，等于null时继续匹配下一项
		/// </summary>
		public virtual decimal? Weight { get; set; }
		/// <summary>
		/// 库存，等于null时继续匹配下一项
		/// </summary>
		public virtual long? Stock { get; set; }
		/// <summary>
		/// 匹配顺序，从小到大
		/// </summary>
		public virtual long MatchOrder { get; set; }
		/// <summary>
		/// 备注，纯文本
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMatchedData() {
			Conditions = new Dictionary<string, object>();
			Affects = new Dictionary<string, object>();
		}
	}

	/// <summary>
	/// 商品匹配数据的数据库结构
	/// </summary>
	[ExportMany]
	public class ProductMatchedDataMap : ClassMap<ProductMatchedData> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMatchedDataMap() {
			Id(d => d.Id);
			References(d => d.Product);
			Map(d => d.Conditions).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(d => d.Affects).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(d => d.Price);
			Map(d => d.PriceCurrency);
			Map(d => d.Weight);
			Map(d => d.Stock);
			Map(d => d.MatchOrder);
			Map(d => d.Remark).Length(0xffff);
		}
	}
}
