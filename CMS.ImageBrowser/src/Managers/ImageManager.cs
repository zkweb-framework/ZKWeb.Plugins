using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Localize;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Managers {
	/// <summary>
	/// 图片管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ImageManager : ICacheCleaner {
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
		protected MemoryCache<string, List<string>> ImageNamesCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ImageManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			ImageQuality = 90;
			ImageExtension = ".jpg";
			ImageThumbnailExtension = ".thumb.jpg";
			ImageBasePathFormat = "/static/cms.image_browser.images/{0}";
			ImageThumbnailSize = new Size(135, 135);
			ImageNamesCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.ImageNamesCacheTime, 15));
			ImageNamesCache = new MemoryCache<string, List<string>>();
		}

		/// <summary>
		/// 获取图片的本地基础路径
		/// </summary>
		/// <returns></returns>
		public virtual string GetImageStorageBasePath(string category) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var basePath = string.Format(ImageBasePathFormat, category);
			return pathManager.GetStorageFullPath(basePath.Substring(1));
		}

		/// <summary>
		/// 获取图片的本地路径
		/// </summary>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extensions">后缀名</param>
		/// <returns></returns>
		public virtual string GetImageStoragePath(string category, string name, string extensions) {
			return PathUtils.SecureCombine(GetImageStorageBasePath(category), name + extensions);
		}

		/// <summary>
		/// 获取图片的网页路径
		/// </summary>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <param name="extensions">后缀名</param>
		/// <returns></returns>
		public virtual string GetImageWebPath(string category, string name, string extensions) {
			return string.Format("{0}/{1}{2}",
				string.Format(ImageBasePathFormat, category), name, extensions);
		}

		/// <summary>
		/// 判断图片是否存在
		/// </summary>
		/// <param name="category">类别</param>
		/// <param name="name">名称，不应该带后缀名</param>
		/// <returns></returns>
		public virtual bool Exists(string category, string name) {
			var imagePath = GetImageStoragePath(category, name, ImageExtension);
			return File.Exists(imagePath);
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
				throw new HttpException(400, new T("Parse uploaded image failed"));
			}
			using (image) {
				// 保存原图
				var imagePath = GetImageStoragePath(category, name, ImageExtension);
				image.SaveAuto(imagePath, ImageQuality);
				// 保存缩略图
				var thumbnailSize = ImageThumbnailSize;
				using (var thumbnailImage = image.Resize(thumbnailSize.Width,
					thumbnailSize.Height, ImageResizeMode.Padding, Color.White)) {
					var thumbnailPath = GetImageStoragePath(category, name, ImageThumbnailExtension);
					thumbnailImage.SaveAuto(thumbnailPath, ImageQuality);
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
			File.Delete(GetImageStoragePath(category, name, ImageExtension));
			// 删除缩略图
			File.Delete(GetImageStoragePath(category, name, ImageThumbnailExtension));
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
			var baseDir = GetImageStorageBasePath(category);
			var names = ImageNamesCache.GetOrDefault(category);
			if (names != null) {
			} else if (!Directory.Exists(baseDir)) {
				names = new List<string>();
			} else {
				names = Directory.EnumerateFiles(baseDir)
					.Where(path => path.EndsWith(ImageExtension) &&
						!path.EndsWith(ImageThumbnailExtension))
					.OrderByDescending(path => File.GetLastWriteTimeUtc(path))
					.Select(path => Path.GetFileNameWithoutExtension(path))
					.ToList();
				ImageNamesCache.Put(category, names, ImageNamesCacheTime);
			}
			return names;
		}

		/// <summary>
		/// 清除缓存
		/// </summary>
		public void ClearCache() {
			ImageNamesCache.Clear();
		}
	}
}
