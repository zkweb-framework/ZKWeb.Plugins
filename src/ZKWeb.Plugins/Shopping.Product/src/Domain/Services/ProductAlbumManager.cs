using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Services {
	/// <summary>
	/// 商品相册管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductAlbumManager : DomainServiceBase {
		/// <summary>
		/// 默认的商品相册图片的路径
		/// </summary>
		public const string DefaultAlbumImagePath = "/static/shopping.product.images/default.jpg";
		/// <summary>
		/// 商品相册图片的路径格式 (Id, 序号, 后缀)
		/// </summary>
		public const string AlbumImagePathFormat = "/static/shopping.product.images/{0}/album_{1}{2}.jpg";
		/// <summary>
		/// 商品相册图片的后缀
		/// </summary>
		public const string AlbumImageExtensions = ".jpg";
		/// <summary>
		/// 商品相册图片质量
		/// </summary>
		public const int AlbumImageQuality = 90;

		/// <summary>
		/// 获取商品相册图片的储存文件
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号，null时返回主图的路径</param>
		/// <param name="type">商品相册的图片类型，原图或缩略图等</param>
		/// <returns></returns>
		public virtual IFileEntry GetAlbumImageStorageFile(
			Guid id, long? index, ProductAlbumImageType type) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var indexString = index.HasValue ? index.Value.ToString() : "main";
			var suffix = type.GetAttribute<ProductAlbumImageSuffixAttribute>().Suffix;
			var path = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			return fileStorage.GetStorageFile(path.Split('/').Skip(1).ToArray());
		}

		/// <summary>
		/// 获取商品相册图片的网页路径，不存在时返回默认路径
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号，null时返回主图的路径</param>
		/// <param name="type">商品相册的图片类型，原图或缩略图等</param>
		/// <param name="defaultPath">不存在时返回的默认路径</param>
		/// <returns></returns>
		public virtual string GetAlbumImageWebPath(
			Guid id, long? index, ProductAlbumImageType type, string defaultPath = DefaultAlbumImagePath) {
			var storageFile = GetAlbumImageStorageFile(id, index, type);
			if (!storageFile.Exists) {
				return defaultPath;
			}
			var indexString = index.HasValue ? index.Value.ToString() : "main";
			var suffix = type.GetAttribute<ProductAlbumImageSuffixAttribute>().Suffix;
			var webPath = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			webPath += "?mtime=" + storageFile.LastWriteTimeUtc.Ticks;
			return webPath;
		}

		/// <summary>
		/// 获取商品相册图片的网页路径列表
		/// 不存在的图片不会包含在列表中
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <returns></returns>
		public virtual List<Dictionary<ProductAlbumImageType, string>> GetExistAlbumImageWebPaths(Guid id) {
			var result = new List<Dictionary<ProductAlbumImageType, string>>();
			for (int i = 1; i <= ProductAlbumUploadData.MaxImageCount; ++i) {
				var dict = new Dictionary<ProductAlbumImageType, string>();
				foreach (ProductAlbumImageType type in Enum.GetValues(typeof(ProductAlbumImageType))) {
					var path = GetAlbumImageWebPath(id, i, type, null);
					if (path != null) {
						dict[type] = path;
					}
				}
				if (dict.Count > 0) {
					result.Add(dict);
				}
			}
			return result;
		}

		/// <summary>
		/// 保存相册图片
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号</param>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <returns></returns>
		public virtual void SaveAlbumImage(Guid id, long index, Stream imageStream) {
			if (imageStream == null) {
				throw new BadRequestException(new T("Please select product album file"));
			}
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new BadRequestException(new T("Parse uploaded image failed"));
			}
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<ProductAlbumSettings>();
			using (image) {
				// 保存原图
				using (var newImage = image.Resize((int)settings.OriginalImageWidth,
					(int)settings.OriginalImageHeight, ImageResizeMode.Cut, Color.White)) {
					var fileEntry = GetAlbumImageStorageFile(id, index, ProductAlbumImageType.Normal);
					using (var stream = fileEntry.OpenWrite()) {
						newImage.SaveAuto(stream, AlbumImageExtensions, AlbumImageQuality);
					}
				}
				// 保存缩略图
				using (var newImage = image.Resize((int)settings.ThumbnailImageWidth,
					(int)settings.ThumbnailImageHeight, ImageResizeMode.Cut, Color.White)) {
					var fileEntry = GetAlbumImageStorageFile(id, index, ProductAlbumImageType.Thumbnail);
					using (var stream = fileEntry.OpenWrite()) {
						newImage.SaveAuto(stream, AlbumImageExtensions, AlbumImageQuality);
					}
				}
			}
		}

		/// <summary>
		/// 删除相册图片
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号，null时删除主图</param>
		public virtual void DeleteAlbumImage(Guid id, long? index) {
			foreach (ProductAlbumImageType type in Enum.GetValues(typeof(ProductAlbumImageType))) {
				GetAlbumImageStorageFile(id, index, type).Delete();
			}
		}

		/// <summary>
		/// 设置相册主图，返回是否设置成功
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号</param>
		/// <returns></returns>
		public virtual bool SetMainAlbumImage(Guid id, long index) {
			// 原图不存在时返回失败，存在时复制覆盖图片到主图
			var fileEntry = GetAlbumImageStorageFile(id, index, ProductAlbumImageType.Normal);
			if (!fileEntry.Exists) {
				return false;
			}
			foreach (ProductAlbumImageType type in Enum.GetValues(typeof(ProductAlbumImageType))) {
				var srcFile = GetAlbumImageStorageFile(id, index, type);
				var dstFile = GetAlbumImageStorageFile(id, null, type);
				if (srcFile.Exists) {
					using (var srcStream = srcFile.OpenRead())
					using (var dstStream = dstFile.OpenWrite()) {
						srcStream.CopyTo(dstStream);
					}
				}
			}
			return true;
		}
	}
}
