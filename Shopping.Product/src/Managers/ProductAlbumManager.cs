using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Config;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.Managers {
	/// <summary>
	/// 商品相册管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductAlbumManager {
		/// <summary>
		/// 默认的商品相册图片的路径
		/// </summary>
		public const string DefaultAlbumImagePath = "/static/shopping.product.images/default.jpg";
		/// <summary>
		/// 商品相册图片的路径格式 (Id, 序号, 后缀)
		/// </summary>
		public const string AlbumImagePathFormat = "/static/shopping.product.images/{0}/album_{1}{2}.jpg";
		/// <summary>
		/// 商品相册图片质量
		/// </summary>
		public const int AlbumImageQuality = 90;

		/// <summary>
		/// 获取商品相册图片的储存路径，路径不一定存在
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号，null时返回主图的路径</param>
		/// <param name="type">商品相册的图片类型，原图或缩略图等</param>
		/// <returns></returns>
		public virtual string GetAlbumImageStoragePath(
			long id, long? index, ProductAlbumImageType type) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var indexString = index.HasValue ? index.Value.ToString() : "main";
			var suffix = type.GetAttribute<ProductAlbumImageSuffixAttribute>().Suffix;
			var path = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			return pathManager.GetStorageFullPath(path.Split('/').Skip(1).ToArray());
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
			long id, long? index, ProductAlbumImageType type, string defaultPath = DefaultAlbumImagePath) {
			var storagePath = GetAlbumImageStoragePath(id, index, type);
			var storagePathInfo = new FileInfo(storagePath);
			if (!storagePathInfo.Exists) {
				return defaultPath;
			}
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var indexString = index.HasValue ? index.Value.ToString() : "main";
			var suffix = type.GetAttribute<ProductAlbumImageSuffixAttribute>().Suffix;
			var webPath = string.Format(AlbumImagePathFormat, id, indexString, suffix);
			webPath += "?mtime=" + storagePathInfo.LastWriteTimeUtc.Ticks;
			return webPath;
		}

		/// <summary>
		/// 获取商品相册图片的网页路径列表
		/// 不存在的图片不会包含在列表中
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <returns></returns>
		public virtual List<Dictionary<ProductAlbumImageType, string>> GetExistAlbumImageWebPaths(long id) {
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
		public virtual void SaveAlbumImage(long id, long index, Stream imageStream) {
			if (imageStream == null) {
				throw new HttpException(400, new T("Please select product album file"));
			}
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new HttpException(400, new T("Parse uploaded image failed"));
			}
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<ProductAlbumSettings>();
			using (image) {
				// 保存原图
				using (var newImage = image.Resize((int)settings.OriginalImageWidth,
					(int)settings.OriginalImageHeight, ImageResizeMode.Cut, Color.White)) {
					var path = GetAlbumImageStoragePath(id, index, ProductAlbumImageType.Normal);
					newImage.SaveAuto(path, AlbumImageQuality);
				}
				// 保存缩略图
				using (var newImage = image.Resize((int)settings.ThumbnailImageWidth,
					(int)settings.ThumbnailImageHeight, ImageResizeMode.Cut, Color.White)) {
					var path = GetAlbumImageStoragePath(id, index, ProductAlbumImageType.Thumbnail);
					newImage.SaveAuto(path, AlbumImageQuality);
				}
			}
		}

		/// <summary>
		/// 删除相册图片
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号，null时删除主图</param>
		public virtual void DeleteAlbumImage(long id, long? index) {
			foreach (ProductAlbumImageType type in Enum.GetValues(typeof(ProductAlbumImageType))) {
				var path = GetAlbumImageStoragePath(id, index, type);
				if (File.Exists(path)) {
					File.Delete(path);
				}
			}
		}

		/// <summary>
		/// 设置相册主图，返回是否设置成功
		/// </summary>
		/// <param name="id">商品Id</param>
		/// <param name="index">图片序号</param>
		/// <returns></returns>
		public virtual bool SetMainAlbumImage(long id, long index) {
			// 原图不存在时返回失败，存在时复制覆盖图片到主图
			var path = GetAlbumImageStoragePath(id, index, ProductAlbumImageType.Normal);
			if (!File.Exists(path)) {
				return false;
			}
			foreach (ProductAlbumImageType type in Enum.GetValues(typeof(ProductAlbumImageType))) {
				var src = GetAlbumImageStoragePath(id, index, type);
				var dst = GetAlbumImageStoragePath(id, null, type);
				if (File.Exists(src)) {
					File.Copy(src, dst, true);
				}
			}
			return true;
		}
	}
}
