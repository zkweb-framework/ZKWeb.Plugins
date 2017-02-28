using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Templating.DynamicContents;
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
		/// 备份当前主题
		/// </summary>
		public virtual void BackupUsingTheme() {
			// TODO: 清理旧的备份文件
			// TODO: 生成备份文件
		}

		/// <summary>
		/// 导出主题
		/// </summary>
		public virtual void ExportTheme() {

		}

		/// <summary>
		/// 导入主题
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
