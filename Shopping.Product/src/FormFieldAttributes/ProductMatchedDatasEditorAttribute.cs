using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes {
	/// <summary>
	/// 商品匹配数据编辑器的属性
	/// 编辑商品时使用
	/// </summary>
	public class ProductMatchedDatasEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 类目Id的字段名称
		/// </summary>
		public string CategoryFieldName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="categoryFieldName">类目Id的字段名称</param>
		public ProductMatchedDatasEditorAttribute(string name, string categoryFieldName) {
			Name = name;
			CategoryFieldName = categoryFieldName;
		}
	}
}
