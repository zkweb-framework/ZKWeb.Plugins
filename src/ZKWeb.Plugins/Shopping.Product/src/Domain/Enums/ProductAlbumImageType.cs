using System;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Enums {
	/// <summary>
	/// 商品相册的图片类型
	/// </summary>
	public enum ProductAlbumImageType {
		/// <summary>
		/// 原图
		/// </summary>
		[ProductAlbumImageSuffix(Suffix = "")]
		Normal,
		/// <summary>
		/// 缩略图
		/// </summary>
		[ProductAlbumImageSuffix(Suffix = ".thumb")]
		Thumbnail
	}

	/// <summary>
	/// 商品相册的图片类型的后缀
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class ProductAlbumImageSuffixAttribute : Attribute {
		/// <summary>
		/// 后缀名
		/// </summary>
		public string Suffix { get; set; }
	}
}
