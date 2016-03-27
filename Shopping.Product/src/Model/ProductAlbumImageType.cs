using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
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
	/// 图片路径的后缀
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class ProductAlbumImageSuffixAttribute : Attribute {
		/// <summary>
		/// 后缀
		/// </summary>
		public string Suffix { get; set; }
	}
}
