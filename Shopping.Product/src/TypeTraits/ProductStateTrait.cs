using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.TypeTraits {
	/// <summary>
	/// 商品状态的特征类
	/// </summary>
	public class ProductStateTrait {
		/// <summary>
		/// 是否显示在商品列表中
		/// </summary>
		public bool VisibleFromProductList { get; set; }
		/// <summary>
		/// 是否可以购买
		/// </summary>
		public bool IsPurchasable { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductStateTrait() {
			VisibleFromProductList = false;
			IsPurchasable = false;
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
