using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes {
	/// <summary>
	/// 商品属性编辑器的属性
	/// </summary>
	public class ProductPropertiesEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 类目Id的字段名称
		/// </summary>
		public string CategoryFieldName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="categoryFieldName">类目Id的字段名称</param>
		public ProductPropertiesEditorAttribute(string name, string categoryFieldName) {
			Name = name;
			CategoryFieldName = categoryFieldName;
		}
	}
}
