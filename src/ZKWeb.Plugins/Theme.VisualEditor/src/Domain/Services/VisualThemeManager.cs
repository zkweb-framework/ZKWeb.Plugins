using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Storage;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services {
	/// <summary>
	/// 可视化编辑主题管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class VisualThemeManager : DomainServiceBase {
		/// <summary>
		/// 最多保留100份备份
		/// </summary>
		public static int KeepBackups = 100;
		/// <summary>
		/// 主题文件夹的名称
		/// </summary>
		public static string ThemeDirectoryName = "themes";
		/// <summary>
		/// 备份主题文件夹的名称
		/// </summary>
		public static string BackupThemeDirectoryName = "theme_backups";

		/// <summary>
		/// 导出当前主题到数据流
		/// 包括以下文件
		///		theme.json 主题信息
		///		App_Data\areas下的文件
		///	不包括以下文件
		///		App_Data\static下的文件
		///		App_Data\templates下的文件
		///		这些文件可以手动添加到包里面
		/// </summary>
		/// <param name="info">主题信息</param>
		/// <param name="stream">数据流</param>
		protected virtual void ExportThemeToStream(VisualThemeInfo info, Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var areasDir = fileStorage.GetStorageDirectory("areas");
			using (var archive = new ZipArchive(stream, ZipArchiveMode.Create)) {
				// 添加主题信息
				var infoEntry = archive.CreateEntry("theme.json");
				using (var writer = new StreamWriter(infoEntry.Open())) {
					writer.Write(JsonConvert.SerializeObject(info));
				}
				// 添加areas下的文件
				foreach (var widgetsFile in areasDir.EnumerateFiles()) {
					var widgetsEntry = archive.CreateEntry("areas/" + widgetsFile.Filename);
					using (var writer = new StreamWriter(widgetsEntry.Open())) {
						writer.Write(widgetsFile.ReadAllText());
					}
				}
			}
		}

		/// <summary>
		/// 从数据流导入主题覆盖当前的主题
		/// </summary>
		/// <param name="stream">数据流</param>
		protected virtual void ImportThemeFromStream(Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			// 删除areas文件夹
			fileStorage.GetStorageDirectory("areas").Delete();
			// 解压缩主题中的所有文件
			// OpenWrite的时候会自动创建文件夹
			using (var archive = new ZipArchive(stream, ZipArchiveMode.Read)) {
				foreach (var entry in archive.Entries) {
					var storageFile = fileStorage.GetStorageFile(entry.FullName);
					using (var writeStream = storageFile.OpenWrite())
					using (var readStream = entry.Open()) {
						readStream.CopyTo(writeStream);
					}
				}
			}
		}

		/// <summary>
		/// 备份当前主题
		/// </summary>
		public virtual void BackupUsingTheme() {
			// 清理旧的备份文件
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var backupThemeDir = fileStorage.GetStorageDirectory(BackupThemeDirectoryName);
			var files = backupThemeDir.EnumerateFiles().OrderByDescending(f => f.Filename).ToList();
			foreach (var file in files.Skip(KeepBackups)) {
				file.Delete();
			}
			// 生成主题信息
			var info = new VisualThemeInfo();
			info.Name = "AutoBackup";
			info.Author = "System";
			info.Version = "1.0";
			// 生成备份文件
			var backupFileName = DateTime.UtcNow.ToClientTime().ToString("yyyyMMdd_HHmmss") + ".zip";
			var backupFile = fileStorage.GetStorageFile(BackupThemeDirectoryName, backupFileName);
			using (var stream = backupFile.OpenWrite()) {
				ExportThemeToStream(info, stream);
			}
		}

		/// <summary>
		/// 导出当前主题
		/// </summary>
		public virtual void ExportTheme() {

		}

		/// <summary>
		/// 导入主题覆盖当前的主题
		/// </summary>
		public virtual void ImportTheme() {

		}

		/// <summary>
		/// 添加主题, 用于上传主题
		/// </summary>
		public virtual void AddTheme() {

		}

		/// <summary>
		/// 获取单个主题
		/// </summary>
		public virtual void GetTheme() {

		}

		/// <summary>
		/// 获取主题列表
		/// </summary>
		public virtual void GetThemes() {

		}

		/// <summary>
		/// 保存可视化编辑的结果
		/// </summary>
		public virtual void SaveEditResult(VisualEditResult result) {
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 备份当前主题
			BackupUsingTheme();
			// 保存各个区域下的模块列表
			foreach (var area in result.Areas) {
				areaManager.SetCustomWidgets(area.AreaId, area.Widgets);
			}
			// 清理模块的缓存
			areaManager.ClearCache();
		}
	}
}
