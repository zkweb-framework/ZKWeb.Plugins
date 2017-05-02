using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Components.ExtraConfigKeys;
using ZKWeb.Storage;

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
		/// 图片后缀，默认是.jpg
		/// </summary>
		public string ImageExtension { get; set; }
		/// <summary>
		/// 图片缩略图的后缀，默认是.thumb.jpg
		/// </summary>
		public string ImageThumbnailExtension { get; set; }
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
		protected IKeyValueCache<string, List<string>> ImageNamesCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ImageManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			ImageQuality = 90;
			ImageExtension = ".jpg";
			ImageThumbnailExtension = ".thumb.jpg";
			ImageBasePathFormat = "/static/cms.image_browser.images/{0}";
			ImageThumbnailSize = new Size(135, 135);
			ImageNamesCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				ImageBrowserExtraConfigKeys.ImageNamesCacheTime, 15));
			ImageNamesCache = cacheFactory.CreateCache<string, List<string>>();
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
		/// <param name="extensions">后缀名</param>
		/// <returns></returns>
		public virtual IFileEntry GetImageStorageFile(string category, string name, string extensions) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var basePath = string.Format(ImageBasePathFormat, category);
			return fileStorage.GetStorageFile(basePath.Substring(1), name + extensions);
		}

		/// <summary>
		/// 判断图片是否存在
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <returns></returns>
		public virtual bool Exists(string category, string name) {
			return GetImageStorageFile(category, name, ImageExtension).Exists;
		}

		/// <summary>
		/// 获取图片的网页路径
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extensions">后缀名</param>
		/// <returns></returns>
		public virtual string GetImageWebPath(string category, string name, string extensions) {
			return string.Format("{0}/{1}{2}",
				string.Format(ImageBasePathFormat, category), name, extensions);
		}

		/// <summary>
		/// 保存图片文件
		/// </summary>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		public virtual void Save(Stream imageStream, string category, string name) {
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new BadRequestException(new T("Parse uploaded image failed"));
			}
			using (image) {
				// 保存原图
				var imageFile = GetImageStorageFile(category, name, ImageExtension);
				using (var stream = imageFile.OpenWrite()) {
					image.SaveAuto(stream, ImageExtension, ImageQuality);
				}
				// 保存缩略图
				var thumbnailSize = ImageThumbnailSize;
				using (var thumbnailImage = image.Resize(thumbnailSize.Width,
					thumbnailSize.Height, ImageResizeMode.Padding, Color.White)) {
					var thumbnailFile = GetImageStorageFile(category, name, ImageThumbnailExtension);
					using (var stream = thumbnailFile.OpenWrite()) {
						thumbnailImage.SaveAuto(stream, ImageExtension, ImageQuality);
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
			// 删除原图
			GetImageStorageFile(category, name, ImageExtension).Delete();
			// 删除缩略图
			GetImageStorageFile(category, name, ImageThumbnailExtension).Delete();
			// 删除缓存
			ImageNamesCache.Remove(category);
		}

		/// <summary>
		/// 查询类别下的图片名称列表
		/// 按最后修改的时间排序
		/// 返回的结果仅图片名称，不带路径和后缀
		/// </summary>
		/// <param name="category">图片类别</param>
		/// <returns></returns>
		public virtual IReadOnlyList<string> Query(string category) {
			// 获取类别对应的文件夹下的所有图片名称
			var directoryEntry = GetImageStorageDirectory(category);
			return ImageNamesCache.GetOrCreate(category, () => {
				if (!directoryEntry.Exists) {
					return new List<string>();
				} else {
					return directoryEntry.EnumerateFiles()
						.Where(file => file.Filename.EndsWith(ImageExtension) &&
							!file.Filename.EndsWith(ImageThumbnailExtension))
						.OrderByDescending(file => file.LastWriteTimeUtc)
						.Select(file => Path.GetFileNameWithoutExtension(file.Filename))
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
