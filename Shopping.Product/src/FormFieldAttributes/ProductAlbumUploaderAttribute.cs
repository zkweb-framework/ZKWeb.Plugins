using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes {
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
