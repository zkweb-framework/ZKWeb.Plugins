using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ZKWeb.Logging;
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
		/// 主题信息的文件名
		/// </summary>
		public static string ThemeInforFilename = "theme.json";
		/// <summary>
		/// 预览图片的文件名
		/// </summary>
		public static string PreviewFilename = "preview.png";

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
		public virtual void ExportThemeToStream(VisualThemeInfo info, Stream stream) {
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var areasDir = fileStorage.GetStorageDirectory("areas");
			using (var archive = new ZipArchive(stream, ZipArchiveMode.Create)) {
				// 添加主题信息
				var infoEntry = archive.CreateEntry(ThemeInforFilename);
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
		public virtual void ImportThemeFromStream(Stream stream) {
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
		/// 从数据流获取主题信息
		/// </summary>
		/// <param name="stream">数据流</param>
		/// <returns></returns>
		public virtual VisualThemeInfo ReadThemeInfoFromStream(Stream stream) {
			using (var archive = new ZipArchive(stream, ZipArchiveMode.Read)) {
				// 读取json文件
				var infoEntry = archive.GetEntry(ThemeInforFilename);
				if (infoEntry == null) {
					return new VisualThemeInfo() { Name = "No Information" };
				}
				string json;
				using (var readStream = infoEntry.Open())
				using (var streamReader = new StreamReader(readStream)) {
					json = streamReader.ReadToEnd();
				}
				var info = JsonConvert.DeserializeObject<VisualThemeInfo>(json);
				// 读取预览图文件
				var previewEntry = archive.GetEntry(PreviewFilename);
				if (previewEntry != null) {
					using (var readSteam = previewEntry.Open())
					using (var memoryStream = new MemoryStream()) {
						readSteam.CopyTo(memoryStream);
						info.PreviewImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
					}
				}
				return info;
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
			var timeString = DateTime.UtcNow.ToClientTime().ToString("yyyyMMdd_HHmmss");
			var info = new VisualThemeInfo();
			info.Name = $"AutoBackup {timeString}";
			info.Author = "System";
			info.Version = "1.0";
			// 生成备份文件
			var backupFileName = $"{timeString}.zip";
			var backupFile = fileStorage.GetStorageFile(BackupThemeDirectoryName, backupFileName);
			using (var stream = backupFile.OpenWrite()) {
				ExportThemeToStream(info, stream);
			}
		}

		/// <summary>
		/// 从指定文件夹读取主题列表
		/// </summary>
		/// <param name="directoryName">文件夹名称</param>
		/// <returns></returns>
		protected virtual IList<VisualThemeInfo> GetThemesFromDirectory(string directoryName) {
			var result = new List<VisualThemeInfo>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var backupThemeDir = fileStorage.GetStorageDirectory(directoryName);
			var files = backupThemeDir.EnumerateFiles().OrderByDescending(f => f.Filename).ToList();
			foreach (var file in files) {
				try {
					using (var stream = file.OpenRead()) {
						var info = ReadThemeInfoFromStream(stream);
						info.Filename = file.Filename;
						result.Add(info);
					}
				} catch (Exception e) {
					var logManager = Application.Ioc.Resolve<LogManager>();
					logManager.LogError($"Read theme '{file.Filename}' failed: '{e}'");
				}
			}
			return result;
		}

		/// <summary>
		/// 获取主题列表
		/// </summary>
		/// <returns></returns>
		public virtual IList<VisualThemeInfo> GetThemes() {
			return GetThemesFromDirectory(ThemeDirectoryName);
		}

		/// <summary>
		/// 获取备份的主题列表
		/// </summary>
		/// <returns></returns>
		public virtual IList<VisualThemeInfo> GetBackupThemes() {
			return GetThemesFromDirectory(BackupThemeDirectoryName);
		}

		/// <summary>
		/// 获取主题的数据流
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public virtual Stream GetThemeStream(string filename) {
			var result = new List<VisualThemeInfo>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var file = fileStorage.GetStorageFile(ThemeDirectoryName, filename);
			return file.OpenRead();
		}

		/// <summary>
		/// 获取备份主题的数据流
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		public virtual Stream GetBackupThemeStream(string filename) {
			var result = new List<VisualThemeInfo>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var file = fileStorage.GetStorageFile(BackupThemeDirectoryName, filename);
			return file.OpenRead();
		}

		/// <summary>
		/// 应用主题
		/// </summary>
		/// <param name="filename">文件名</param>
		public virtual void ApplyTheme(string filename) {
			// 应用普通主题前应该备份当前的主题
			BackupUsingTheme();
			using (var stream = GetThemeStream(filename)) {
				ImportThemeFromStream(stream);
			}
		}

		/// <summary>
		/// 应用备份主题
		/// </summary>
		/// <param name="filename">文件名</param>
		public virtual void ApplyBackupTheme(string filename) {
			using (var stream = GetBackupThemeStream(filename)) {
				ImportThemeFromStream(stream);
			}
		}

		/// <summary>
		/// 上传主题
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <param name="stream">数据流</param>
		public virtual void UploadTheme(string filename, Stream stream) {
			var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
			var result = new List<VisualThemeInfo>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			int index = 0;
			var saveFilename = $"{filenameWithoutExtension}.zip";
			IFileEntry file = fileStorage.GetStorageFile(ThemeDirectoryName, saveFilename);
			while (file.Exists) {
				saveFilename = $"{filenameWithoutExtension}({++index}).zip";
				file = fileStorage.GetStorageFile(ThemeDirectoryName, saveFilename);
			}
			using (var fileStream = file.OpenWrite()) {
				stream.CopyTo(fileStream);
			}
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
