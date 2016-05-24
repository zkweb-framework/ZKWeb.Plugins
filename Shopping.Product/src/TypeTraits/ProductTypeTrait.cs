using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.TypeTraits {
	/// <summary>
	/// 商品类型的特征类
	/// </summary>
	public class ProductTypeTrait {
		/// <summary>
		/// 返回指定商品类型的特征
		/// </summary>
		/// <param name="type">商品类型</param>
		/// <returns></returns>
		public ProductTypeTrait For(Type type) {
			var trait = Application.Ioc.Resolve<ProductTypeTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			return trait ?? new ProductTypeTrait();
		}

		/// <summary>
		/// 返回指定商品类型的特征
		/// </summary>
		/// <typeparam name="T">商品类型</typeparam>
		/// <returns></returns>
		public ProductTypeTrait For<T>() {
			return For(typeof(T));
		}
	}
}
