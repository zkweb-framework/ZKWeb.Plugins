using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 商品相册上传器的属性
	/// 编辑商品时使用
	/// </summary>
	public class ProductAlbumUploaderAttribute : FileUploaderFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public ProductAlbumUploaderAttribute(string name) : base(name) {
			Name = name;
		}
	}
}
