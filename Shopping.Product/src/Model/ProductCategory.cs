using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品类目
	/// 这个对象中的值生成后不应该修改
	/// </summary>
	public class ProductCategory {
		/// <summary>
		/// 类目Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 上级类目Id，不存在时等于0
		/// </summary>
		public long ParentId { get; set; }
		/// <summary>
		/// 类目名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 类目下的属性Id列表
		/// Id对应的属性中必须有任意一个的ParentCategoryIds包含这个类目的Id
		/// 否则应该忽略该属性
		/// </summary>
		public IList<long> PropertyIds { get; set; }

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}
	}
}
