using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Managers {
	/// <summary>
	/// 图片管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ImageManager {
		/// <summary>
		/// 图片保存质量，默认是90
		/// </summary>
		public virtual int ImageQuality { get { return 90; } }
		/// <summary>
		/// 图片后缀
		/// </summary>
		public virtual string ImageExtension { get { return ".jpg"; } }
		/// <summary>
		/// 图片缩略图的后缀
		/// </summary>
		public virtual string ImageThumbnailExtension { get { return ".thumb.jpg"; } }
		/// <summary>
		/// 图片基础路径的格式
		/// 参数: 类别
		/// </summary>
		public virtual string ImageBasePathFormat { get { return "/static/cms.image_browser.images/{0}"; } }
		/// <summary>
		/// 图片缩略图的大小
		/// </summary>
		public virtual Size ImageThumbnailSize { get { return new Size(135, 135); } }

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
		/// 保存图片文件
		/// </summary>
		/// <param name="imageStream">读取图片的数据流</param>
		/// <param name="category">类别</param>
		/// <param name="filename">文件名，指定的后缀名会被忽略</param>
		public virtual void Save(Stream imageStream, string category, string filename) {
			// 忽略原有的后缀名
			filename = Path.GetFileNameWithoutExtension(filename);
			// 读取图片
			Image image;
			try {
				image = Image.FromStream(imageStream);
			} catch {
				throw new HttpException(400, new T("Parse uploaded image failed"));
			}
			using (image) {
				// 保存原图
				var imagePath = GetImageStoragePath(category, filename, ImageExtension);
				Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
				image.SaveAuto(imagePath, ImageQuality);
				// 保存缩略图
				var thumbnailSize = ImageThumbnailSize;
				using (var thumbnailImage = image.Resize(thumbnailSize.Width,
					thumbnailSize.Height, ImageResizeMode.Padding, Color.White)) {
					var thumbnailPath = GetImageStoragePath(category, filename, ImageThumbnailExtension);
					Directory.CreateDirectory(Path.GetDirectoryName(thumbnailPath));
					thumbnailImage.SaveAuto(thumbnailPath, ImageQuality);
				}
			}
		}

		/// <summary>
		/// 删除图片文件
		/// </summary>
		/// <param name="category">类别</param>
		/// <param name="filename">文件名，指定的后缀名会被忽略</param>
		public virtual void Remove(string category, string filename) {
			// 忽略原有的后缀名
			filename = Path.GetFileNameWithoutExtension(filename);
			// 删除原图
			File.Delete(GetImageStoragePath(category, filename, ImageExtension));
			// 删除缩略图
			File.Delete(GetImageStoragePath(category, filename, ImageThumbnailExtension));
		}
	}
}
