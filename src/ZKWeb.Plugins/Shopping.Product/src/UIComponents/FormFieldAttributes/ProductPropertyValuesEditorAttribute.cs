using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 商品属性值的编辑器的属性
	/// 编辑商品属性时使用
	/// </summary>
	public class ProductPropertyValuesEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public ProductPropertyValuesEditorAttribute(string name) {
			Name = name;
		}
	}
}
