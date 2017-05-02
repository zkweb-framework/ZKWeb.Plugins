using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Enums;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services {
	/// <summary>
	/// 商品评价相册管理器
	/// </summary>
	[ExportMany]
	public class ProductRatingAlbumManager : DomainServiceBase {
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
		/// <summary>
		/// 缩略图宽度
		/// </summary>
		public static int ThumbnailImageWidth = 50;
		/// <summary>
		/// 缩略图高度
		/// </summary>
		public static int ThumbnailImageHeight = 50;

		/// <summary>
		/// 获取商品评价相册图片的储存文件
		/// </summary>
		/// <param name="id">订单商品Id</param>
		/// <param name="index">图片序号</param>
		/// <param name="type">商品相册的图片类型，原图或缩略图等</param>
		/// <returns></returns>
		public virtual IFileEntry GetAlbumImageStorageFile(
			Guid id, long index, ProductRatingAlbumImageType type) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var indexString = index.ToString();
			var suffix = type.GetAttribute<ProductRatingAlbumImageSuffixAttribute>().Suffix;
			var path = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			return fileStorage.GetStorageFile(path.Split('/').Skip(1).ToArray());
		}

		/// <summary>
		/// 获取商品评价相册图片的网页路径，不存在时返回null
		/// </summary>
		/// <param name="id">订单商品Id</param>
		/// <param name="index">图片序号</param>
		/// <param name="type">商品相册的图片类型，原图或缩略图等</param>
		/// <returns></returns>
		public virtual string GetAlbumImageWebPath(
			Guid id, long index, ProductRatingAlbumImageType type) {
			var storageFile = GetAlbumImageStorageFile(id, index, type);
			if (!storageFile.Exists) {
				return null;
			}
			var indexString = index.ToString();
			var suffix = type.GetAttribute<ProductRatingAlbumImageSuffixAttribute>().Suffix;
			var webPath = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			webPath += "?mtime=" + storageFile.LastWriteTimeUtc.Ticks;
			return webPath;
		}

		/// <summary>
		/// 获取商品评价相册图片的网页路径列表
		/// 不存在的图片不会包含在列表中
		/// </summary>
		/// <param name="id">订单商品Id</param>
		/// <returns></returns>
		public virtual List<Dictionary<ProductRatingAlbumImageType, string>> GetExistAlbumImageWebPaths(Guid id) {
			var result = new List<Dictionary<ProductRatingAlbumImageType, string>>();
			for (int i = 1; ; ++i) {
				var dict = new Dictionary<ProductRatingAlbumImageType, string>();
				foreach (ProductRatingAlbumImageType type in
					Enum.GetValues(typeof(ProductRatingAlbumImageType))) {
					var path = GetAlbumImageWebPath(id, i, type);
					if (path != null) {
						dict[type] = path;
					}
				}
				if (dict.Count > 0) {
					result.Add(dict);
				} else {
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 保存商品评价相册图片
		/// </summary>
		/// <param name="id">订单商品Id</param>
		/// <param name="index">图片序号</param>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <returns></returns>
		public virtual void SaveAlbumImage(Guid id, long index, Stream imageStream) {
			if (imageStream == null) {
				throw new BadRequestException(new T("Please select product rating album file"));
			}
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new BadRequestException(new T("Parse uploaded image failed"));
			}
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			using (image) {
				// 保存原图
				{
					var fileEntry = GetAlbumImageStorageFile(id, index, ProductRatingAlbumImageType.Normal);
					using (var stream = fileEntry.OpenWrite()) {
						image.SaveAuto(stream, AlbumImageExtensions, AlbumImageQuality);
					}
				}
				// 保存缩略图
				using (var newImage = image.Resize(
					ThumbnailImageWidth, ThumbnailImageHeight, ImageResizeMode.Cut, Color.White)) {
					var fileEntry = GetAlbumImageStorageFile(id, index, ProductRatingAlbumImageType.Thumbnail);
					using (var stream = fileEntry.OpenWrite()) {
						newImage.SaveAuto(stream, AlbumImageExtensions, AlbumImageQuality);
					}
				}
			}
		}

		/// <summary>
		/// 删除相册图片
		/// </summary>
		/// <param name="id">订单商品Id</param>
		/// <param name="index">图片序号，null时删除主图</param>
		public virtual void DeleteAlbumImage(Guid id, long index) {
			foreach (ProductRatingAlbumImageType type in Enum.GetValues(typeof(ProductRatingAlbumImageType))) {
				GetAlbumImageStorageFile(id, index, type).Delete();
			}
		}
	}
}
