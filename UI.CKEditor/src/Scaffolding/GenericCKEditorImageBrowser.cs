using DryIoc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.UI.CKEditor.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.UI.CKEditor.src.Scaffolding {
	/// <summary>
	/// 通用的图片浏览器
	/// 导出时需要指定Key等于图片上传类目
	/// 例：
	/// [ExportMany(ContractKey = "Article")]
	/// public class ArticleCKEditorImageBrowser : GenericCKEditorImageBrowser {
	///		TODO: 待补充
	/// }
	/// </summary>
	public abstract class GenericCKEditorImageBrowser : ICKEditorImageBrowser {
		/// <summary>
		/// 图片的基础路径
		/// 推荐以static开始，否则需要自己编写控制器显示图片
		/// </summary>
		public abstract string[] BasePath { get; }
		/// <summary>
		/// 要求的用户类型
		/// </summary>
		public abstract UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 要求的权限
		/// </summary>
		public abstract string[] RequiredPrivileges { get; }
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
		/// 获取基础路径
		/// </summary>
		/// <returns></returns>
		protected virtual string GetFullBasePath() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			return pathManager.GetStorageFullPath(BasePath);
		}

		/// <summary>
		/// 获取图片的本地路径
		/// </summary>
		protected virtual string GetImageStoragePath(string name, string extensions) {
			return PathUtils.SecureCombine(GetFullBasePath(), name + extensions);
		}

		/// <summary>
		/// 保存图片文件
		/// </summary>
		public virtual void Save(Image image, string name) {
			// 保存原图
			image.SaveAuto(GetImageStoragePath(name, ImageExtension), ImageQuality);
			// 保存缩略图
			image.SaveAuto(GetImageStoragePath(name, ImageThumbnailExtension), ImageQuality);
		}

		/// <summary>
		/// 删除图片文件
		/// </summary>
		public virtual void Remove(string name) {
			// 删除原图
			File.Delete(GetImageStoragePath(name, ImageExtension));
			// 删除缩略图
			File.Delete(GetImageStoragePath(name, ImageThumbnailExtension));
		}

		/// <summary>
		/// 搜索图片文件
		/// </summary>
		public virtual AjaxTableSearchResponse Search(AjaxTableSearchRequest request) {
			var response = new AjaxTableSearchResponse();
			return response;
		}
	}
}
