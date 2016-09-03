using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 商品关联的属性值的编辑器的属性
	/// 编辑商品时使用
	/// </summary>
	public class ProductToProperyValuesEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 类目Id的字段名称
		/// </summary>
		public string CategoryFieldName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="categoryFieldName">类目Id的字段名称</param>
		public ProductToProperyValuesEditorAttribute(string name, string categoryFieldName) {
			Name = name;
			CategoryFieldName = categoryFieldName;
		}
	}
}
