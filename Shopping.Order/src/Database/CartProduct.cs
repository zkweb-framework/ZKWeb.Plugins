using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 购物车商品
	/// 可以属于用户也可以属于会话，属于会话时需要指定比较短的过期时间
	/// 类型可以是默认也可以是立刻购买，立刻购买时需要指定比较短的过期时间
	/// 删除：直接从数据库中删除
	/// </summary>
	public class CartProduct {
		/// <summary>
		/// 数据Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 类型
		/// 默认或立刻购买
		/// </summary>
		public virtual CardProductType Type { get; set; }
		/// <summary>
		/// 买家用户，没有时等于null
		/// </summary>
		public virtual User Buyer { get; set; }
		/// <summary>
		/// 买家会话，已有买家用户时等于null
		/// 这里不关联会话对象避免删除问题
		/// </summary>
		public virtual long? BuyerSessionId { get; set; }
		/// <summary>
		/// 商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 购买数量
		/// </summary>
		public virtual long Count { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// 包含规格等信息，这里会包含购买数量但不应该使用这里的数量
		/// </summary>
		public virtual Dictionary<string, object> MatchParameters { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
		public virtual DateTime ExpireTime { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CartProduct() {
			MatchParameters = new Dictionary<string, object>();
		}
	}

	/// <summary>
	/// 购物车商品的数据库结构
	/// </summary>
	[ExportMany]
	public class CartProductMap : ClassMap<CartProduct> {
		/// <summary>
		/// 初始化
		/// </summary>
		public CartProductMap() {
			Id(c => c.Id);
			Map(c => c.Type);
			References(c => c.Buyer).Cascade.Delete();
			Map(c => c.BuyerSessionId).Index("Idx_BuyerSessionId");
			References(c => c.Product).Cascade.Delete();
			Map(c => c.Count);
			Map(c => c.MatchParameters).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(c => c.CreateTime);
			Map(c => c.ExpireTime).Index("Idx_ExpireTime");
		}
	}
}
