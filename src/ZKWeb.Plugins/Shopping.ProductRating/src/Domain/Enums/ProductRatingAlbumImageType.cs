using System;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Enums {
	/// <summary>
	/// 商品相册的图片类型
	/// </summary>
	public enum ProductRatingAlbumImageType {
		/// <summary>
		/// 原图
		/// </summary>
		[ProductRatingAlbumImageSuffix(Suffix = "")]
		Normal,
		/// <summary>
		/// 缩略图
		/// </summary>
		[ProductRatingAlbumImageSuffix(Suffix = ".thumb")]
		Thumbnail
	}

	/// <summary>
	/// 商品相册的图片类型的后缀
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class ProductRatingAlbumImageSuffixAttribute : Attribute {
		/// <summary>
		/// 后缀名
		/// </summary>
		public string Suffix { get; set; }
	}
}
