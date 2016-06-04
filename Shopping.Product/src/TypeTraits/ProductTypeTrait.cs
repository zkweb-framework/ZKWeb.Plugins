using DotLiquid;
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
	public class ProductTypeTrait : ILiquidizable {
		/// <summary>
		/// 附加特征
		/// </summary>
		public Dictionary<string, object> Extra { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductTypeTrait() {
			Extra = new Dictionary<string, object>();
		}

		/// <summary>
		/// 支持在模板中使用
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { Extra };
		}

		/// <summary>
		/// 返回指定商品类型的特征
		/// </summary>
		/// <param name="type">商品类型</param>
		/// <returns></returns>
		public static ProductTypeTrait For(Type type) {
			var trait = Application.Ioc.Resolve<ProductTypeTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			return trait ?? new ProductTypeTrait();
		}

		/// <summary>
		/// 返回指定商品类型的特征
		/// </summary>
		/// <typeparam name="T">商品类型</typeparam>
		/// <returns></returns>
		public static ProductTypeTrait For<T>() {
			return For(typeof(T));
		}
	}
}
