using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品属性值
	/// 这个对象中的值生成后不应该修改
	/// </summary>
	public class ProductPropertyValue {
		/// <summary>
		/// 属性值Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 属性值名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 显示名称
		/// </summary>
		public override string ToString() {
			return Name;
		}
	}
}
