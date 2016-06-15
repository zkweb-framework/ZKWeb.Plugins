using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.TypeTraits {
	/// <summary>
	/// 商品状态的特征类
	/// </summary>
	public class ProductStateTrait : ILiquidizable {
		/// <summary>
		/// 是否显示在商品列表中
		/// </summary>
		public bool VisibleFromProductList { get; set; }
		/// <summary>
		/// 是否可以购买
		/// </summary>
		public bool IsPurchasable { get; set; }
		/// <summary>
		/// 附加特征
		/// </summary>
		public Dictionary<string, object> Extra { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductStateTrait() {
			VisibleFromProductList = false;
			IsPurchasable = false;
			Extra = new Dictionary<string, object>();
		}

		/// <summary>
		/// 支持在模板中使用
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { VisibleFromProductList, IsPurchasable, Extra };
		}

		/// <summary>
		/// 返回指定商品状态的特征
		/// </summary>
		/// <param name="type">商品状态</param>
		/// <returns></returns>
		public static ProductStateTrait For(Type type) {
			var trait = Application.Ioc.Resolve<ProductStateTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			return trait ?? new ProductStateTrait();
		}

		/// <summary>
		/// 返回指定商品状态的特征
		/// </summary>
		/// <typeparam name="T">商品状态</typeparam>
		/// <returns></returns>
		public static ProductStateTrait For<T>() {
			return For(typeof(T));
		}
	}
}
