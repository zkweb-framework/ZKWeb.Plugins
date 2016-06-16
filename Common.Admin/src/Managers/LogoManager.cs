using System.Drawing;
using System.IO;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Managers {
	/// <summary>
	/// Logo管理器
	/// </summary>
	[ExportMany]
	public class LogoManager {
		/// <summary>
		/// 前台Logo的路径
		/// </summary>
		public static readonly string[] FrontPageLogoPath = new[] { "static", "common.base.images", "logo.png" };
		/// <summary>
		/// 后台Logo的路径
		/// </summary>
		public static readonly string[] AdminPanelLogoPath = new[] { "static", "common.admin.images", "logo.png" };
		/// <summary>
		/// 页面图标
		/// </summary>
		public static readonly string[] FaviconPath = new[] { "static", "favicon.ico" };
		/// <summary>
		/// Logo图片质量
		/// </summary>
		public const int LogoImageQuality = 100;

		/// <summary>
		/// 保存前台Logo
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveFrontPageLogo(Stream stream) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(FrontPageLogoPath);
			using (var image = Image.FromStream(stream)) {
				image.SaveAuto(path, LogoImageQuality);
			}
		}

		/// <summary>
		/// 保存后台Logo
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveAdminPanelLogo(Stream stream) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(AdminPanelLogoPath);
			using (var image = Image.FromStream(stream)) {
				image.SaveAuto(path, LogoImageQuality);
			}
		}

		/// <summary>
		/// 保存页面图标
		/// </summary>
		/// <param name="stream">图片的数据流</param>
		public virtual void SaveFavicon(Stream stream) {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(FaviconPath);
			using (var image = Image.FromStream(stream)) {
				image.SaveAuto(path, LogoImageQuality);
			}
		}

		/// <summary>
		/// 恢复默认的前台Logo
		/// </summary>
		public virtual void RestoreDefaultFrontPageLogo() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(FrontPageLogoPath);
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}

		/// <summary>
		/// 恢复默认的后台Logo
		/// </summary>
		public virtual void RestoreDefaultAdminPageLogo() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(AdminPanelLogoPath);
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}

		/// <summary>
		/// 恢复默认的页面图标
		/// </summary>
		public virtual void RestoreDefaultFavicon() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetStorageFullPath(FaviconPath);
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}
	}
}
