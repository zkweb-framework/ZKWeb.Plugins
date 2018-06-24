using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Components.ExtraConfigKeys;
using ZKWeb.Storage;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Domain.Structs;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Domain.Services {
	/// <summary>
	/// 图片管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ImageManager : DomainServiceBase, ICacheCleaner {
		/// <summary>
		/// 图片保存质量，默认是90
		/// </summary>
		public int ImageQuality { get; set; }
		/// <summary>
		/// 默认的图片后缀，默认是.jpg
		/// </summary>
		public string ImageExtension { get; set; }
		/// <summary>
		/// 默认的图片缩略图的后缀，默认是.thumb
		/// </summary>
		public string ImageThumbnailSuffix { get; set; }
		/// <summary>
		/// 所有的图片后缀，默认是[.jpg, .jpeg, .bmp, .png, .gif]
		/// </summary>
		public string[] AllImageExtensions { get; set; }
		/// <summary>
		/// 图片基础路径的格式
		/// 参数: 类别
		/// </summary>
		public string ImageBasePathFormat { get; set; }
		/// <summary>
		/// 图片缩略图的大小，默认是135x135
		/// </summary>
		public Size ImageThumbnailSize { get; set; }
		/// <summary>
		/// 同一类别下的图片名的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ImageNamesCacheTime { get; set; }
		/// <summary>
		/// 同一类别下的图片名的缓存
		/// { 类别: [图片名, ...], ... }
		/// </summary>
		protected IKeyValueCache<string, IReadOnlyList<ImageQueryResult>> ImageNamesCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ImageManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			ImageQuality = 90;
			ImageExtension = ".jpg";
			ImageThumbnailSuffix = ".thumb";
			AllImageExtensions = new[] { ".jpg", "jpeg", ".bmp", ".png", ".gif" };
			ImageBasePathFormat = "/static/cms.image_browser.images/{0}";
			ImageThumbnailSize = new Size(135, 135);
			ImageNamesCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ImageBrowserExtraConfigKeys.ImageNamesCacheTime, 15));
			ImageNamesCache = cacheFactory.CreateCache<string, IReadOnlyList<ImageQueryResult>>();
		}

		/// <summary>
		/// 获取图片的储存文件夹
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <returns></returns>
		public virtual IDirectoryEntry GetImageStorageDirectory(string category) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var basePath = string.Format(ImageBasePathFormat, category);
			return fileStorage.GetStorageDirectory(basePath.Substring(1));
		}


		/// <summary>
		/// 获取图片的储存文件
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <returns></returns>
		public virtual IFileEntry GetImageStorageFile(string category, string name) {
			return GetImageStorageFile(category, name, ImageExtension);
		}

		/// <summary>
		/// 获取图片的储存文件
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extension">后缀名</param>
		/// <returns></returns>
		public virtual IFileEntry GetImageStorageFile(string category, string name, string extension) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var basePath = string.Format(ImageBasePathFormat, category);
			return fileStorage.GetStorageFile(basePath.Substring(1), name + extension);
		}

		/// <summary>
		/// 判断图片是否存在
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <returns></returns>
		public virtual bool Exist(string category, string name) {
			return Exist(category, name, ImageExtension);
		}

		/// <summary>
		/// 判断图片是否存在
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extension">后缀名</param>
		/// <returns></returns>
		public virtual bool Exist(string category, string name, string extension) {
			return GetImageStorageFile(category, name, extension).Exists;
		}

		/// <summary>
		/// 获取图片的网页路径
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <returns></returns>
		public virtual string GetImageWebPath(string category, string name) {
			return GetImageWebPath(category, name, ImageExtension);
		}

		/// <summary>
		/// 获取图片的网页路径
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extension">后缀名</param>
		/// <returns></returns>
		public virtual string GetImageWebPath(string category, string name, string extension) {
			return string.Format("{0}/{1}{2}",
				string.Format(ImageBasePathFormat, category), name, extension);
		}


		/// <summary>
		/// 保存图片文件
		/// </summary>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		public virtual void Save(Stream imageStream, string category, string name) {
			Save(imageStream, category, name, ImageExtension);
		}

		/// <summary>
		/// 保存图片文件
		/// </summary>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extension">后缀名</param>
		public virtual void Save(Stream imageStream, string category, string name, string extension) {
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new BadRequestException(new T("Parse uploaded image failed"));
			}
			using (image) {
				// 保存原图
				var imageFile = GetImageStorageFile(category, name, extension);
				using (var stream = imageFile.OpenWrite()) {
					image.SaveAuto(stream, extension, ImageQuality);
				}
				// 保存缩略图
				var thumbnailSize = ImageThumbnailSize;
				var backgroundColor = extension == ".png" ? Color.Transparent : Color.Wheat;
				using (var thumbnailImage = image.Resize(thumbnailSize.Width,
					thumbnailSize.Height, ImageResizeMode.Padding, backgroundColor)) {
					var thumbnailFile = GetImageStorageFile(category, name, ImageThumbnailSuffix + extension);
					using (var stream = thumbnailFile.OpenWrite()) {
						thumbnailImage.SaveAuto(stream, extension, ImageQuality);
					}
				}
			}
			// 删除缓存
			ImageNamesCache.Remove(category);
		}

		/// <summary>
		/// 删除图片文件
		/// </summary>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		public virtual void Remove(string category, string name) {
			Remove(category, name, ImageExtension);
		}

		/// <summary>
		/// 删除图片文件
		/// </summary>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extension">后缀名</param>
		public virtual void Remove(string category, string name, string extension) {
			// 删除原图
			GetImageStorageFile(category, name, extension).Delete();
			// 删除缩略图
			GetImageStorageFile(category, name, ImageThumbnailSuffix + extension).Delete();
			// 删除缓存
			ImageNamesCache.Remove(category);
		}

		/// <summary>
		/// 查询类别下的图片名称列表
		/// 按最后修改的时间排序
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <returns></returns>
		public virtual IReadOnlyList<ImageQueryResult> Query(string category) {
			// 获取类别对应的文件夹下的所有图片名称
			var directoryEntry = GetImageStorageDirectory(category);
			return ImageNamesCache.GetOrCreate(category, () => {
				if (!directoryEntry.Exists) {
					return new List<ImageQueryResult>();
				} else {
					return directoryEntry.EnumerateFiles()
						.Where(file => AllImageExtensions.Any(e =>
							file.Filename.EndsWith(e) &&
							!file.Filename.EndsWith(ImageThumbnailSuffix + e)))
						.OrderByDescending(file => file.LastWriteTimeUtc)
						.Select(file => new ImageQueryResult() {
							Category = category,
							FilenameWithoutExtension = Path.GetFileNameWithoutExtension(file.Filename),
							Extension = Path.GetExtension(file.Filename)
						})
						.ToList();
				}
			}, ImageNamesCacheTime);
		}

		/// <summary>
		/// 清除缓存
		/// </summary>
		public void ClearCache() {
			ImageNamesCache.Clear();
		}
	}
}
