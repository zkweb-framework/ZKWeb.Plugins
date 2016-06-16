using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.Config {
	/// <summary>
	/// 商品相册设置
	/// </summary>
	[GenericConfig("Shopping.Product.ProductAlbumSettings", CacheTime = 15)]
	public class ProductAlbumSettings {
		/// <summary>
		/// 原图宽度
		/// </summary>
		public long OriginalImageWidth { get; set; }
		/// <summary>
		/// 原图高度
		/// </summary>
		public long OriginalImageHeight { get; set; }
		/// <summary>
		/// 缩略图宽度
		/// </summary>
		public long ThumbnailImageWidth { get; set; }
		/// <summary>
		/// 缩略图高度
		/// </summary>
		public long ThumbnailImageHeight { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductAlbumSettings() {
			OriginalImageWidth = 800;
			OriginalImageHeight = 800;
			ThumbnailImageWidth = 400;
			ThumbnailImageHeight = 400;
		}
	}
}
