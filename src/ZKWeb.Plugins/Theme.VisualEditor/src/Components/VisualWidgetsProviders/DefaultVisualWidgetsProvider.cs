using System.Collections.Generic;
using System.IO;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Storage;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualWidgetsProviders {
	/// <summary>
	/// 获取可以在可视化编辑中使用的模块
	/// 获取流程:
	/// - 遍历App_Data\templates，查找widget文件
	/// - 相反顺序遍历插件，遍历插件\templates，查找widget文件
	/// 不遍历设备专用的模板目录，
	/// 原因是设备专用的模块文件只应用于重载修改样式，同时减少处理的复杂度
	/// 同样的自定义文件储存中的模块也不遍历，如果有需要请另外编写提供器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class DefaultVisualWidgetsProvider : IVisualWidgetsProvider {
		/// <summary>
		/// 获取模块的路径
		/// 替换\到/并去除.widget后缀名
		/// </summary>
		private static string GetWidgetPath(string dir, string fullPath) {
			var path = fullPath.Substring(dir.Length);
			path = path.Replace('\\', '/');
			return Path.ChangeExtension(path, null);
		}

		/// <summary>
		/// 获取可以在可视化编辑中使用的模块列表
		/// </summary>
		public IEnumerable<VisualWidgetInfo> GetWidgets() {
			// { 路径: 模块信息 }
			// 只添加最开始遍历到的模块，防止添加被重载的模块
			var widgetInfos = new Dictionary<string, VisualWidgetInfo>();
			var pathConfig = Application.Ioc.Resolve<LocalPathConfig>();
			var pluginManager = Application.Ioc.Resolve<PluginManager>();
			var templateAreaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 遍历App_Data\templates，查找widget文件
			var customTemplateDir = PathUtils.SecureCombine(
				pathConfig.AppDataDirectory, pathConfig.TemplateDirectoryName);
			if (Directory.Exists(customTemplateDir)) {
				foreach (var path in Directory.EnumerateFiles(
					customTemplateDir, "*.widget", SearchOption.AllDirectories)) {
					var widgetPath = GetWidgetPath(customTemplateDir, path);
					if (widgetInfos.ContainsKey(widgetPath)) {
						continue;
					}
					var widgetInfo = templateAreaManager.GetWidgetInfo(widgetPath);
					widgetInfos[widgetPath] = new VisualWidgetInfo("Custom", widgetInfo);
				}
			}
			// 相反顺序遍历插件，遍历插件\templates，查找widget文件
			foreach (var plugin in pluginManager.Plugins) {
				var pluginTemplateDir = PathUtils.SecureCombine(
					plugin.Directory, pathConfig.TemplateDirectoryName);
				if (!Directory.Exists(pluginTemplateDir)) {
					continue;
				}
				foreach (var path in Directory.EnumerateFiles(
					pluginTemplateDir, "*.widget", SearchOption.AllDirectories)) {
					var widgetPath = GetWidgetPath(pluginTemplateDir, path);
					if (widgetInfos.ContainsKey(widgetPath)) {
						continue;
					}
					var widgetInfo = templateAreaManager.GetWidgetInfo(widgetPath);
					widgetInfos[widgetPath] = new VisualWidgetInfo(plugin.Name, widgetInfo);
				}
			}
			return widgetInfos.Values;
		}
	}
}
