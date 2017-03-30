using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services {
	/// <summary>
	/// 商品评价相册管理器
	/// </summary>
	[ExportMany]
	public class ProductRatingAlbumManager : DomainServiceBase {
		/// <summary>
		/// 默认的商品评价相册图片的路径
		/// </summary>
		public const string DefaultAlbumImagePath = "/static/shopping.productrating.images/default.jpg";
		/// <summary>
		/// 商品评价相册图片的路径格式 (Id, 序号, 后缀)
		/// </summary>
		public const string AlbumImagePathFormat = "/static/shopping.productrating.images/{0}/album_{1}{2}.jpg";
		/// <summary>
		/// 商品评价相册图片的后缀
		/// </summary>
		public const string AlbumImageExtensions = ".jpg";
		/// <summary>
		/// 商品评价相册图片质量
		/// </summary>
		public const int AlbumImageQuality = 90;


	}
}
