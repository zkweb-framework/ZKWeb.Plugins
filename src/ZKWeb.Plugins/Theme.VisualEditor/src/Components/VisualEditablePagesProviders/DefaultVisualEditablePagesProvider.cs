using System.Collections.Generic;
using System.ComponentModel;
using System.FastReflection;
using System.Linq;
using System.Reflection;
using ZKWeb.Cache;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualEditablePagesProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWeb.Web;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualEditablePagesProviders {
	/// <summary>
	/// 获取可视化编辑中可以编辑的页面
	/// 获取流程:
	/// - 遍历所有插件
	/// - 查找所有非abstract的控制器类型
	/// - 查找控制器类型中带Action和Description属性, 且Action属性的Method是GET的函数
	/// </summary>
	[ExportMany, SingletonReuse]
	public class DefaultVisualEditablePagesProvider :
		IVisualEditablePagesProvider, ICacheCleaner {
		/// <summary>
		/// 可编辑的页面Url集合
		/// 调用GetEditablePages后自动更新
		/// </summary>
		private ISet<string> EditablePagesSet { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DefaultVisualEditablePagesProvider() {
			EditablePagesSet = null;
		}

		/// <summary>
		/// 获取可编辑的页面列表
		/// </summary>
		public IEnumerable<VisualEditablePageInfo> GetEditablePages() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			var pluginManager = Application.Ioc.Resolve<PluginManager>();
			var editablePages = new List<VisualEditablePageInfo>();
			foreach (var plugin in pluginManager.Plugins) {
				// 获取所有插件
				if (plugin.Assembly == null) {
					continue;
				}
				foreach (var type in plugin.Assembly.GetTypes()) {
					// 获取所有控制器类型
					var typeInfo = type.GetTypeInfo();
					if (!typeInfo.IsPublic ||
						typeInfo.IsInterface ||
						typeInfo.IsAbstract ||
						!type.FastGetInterfaces().Any(x => x == typeof(IController))) {
						continue;
					}
					foreach (var method in type.FastGetMethods()) {
						// 获取所有函数
						var descriptionAttribute = method.GetAttribute<DescriptionAttribute>();
						var actionAttribute = method.GetAttributes<ActionAttribute>()
							.FirstOrDefault(a => a.Method == HttpMethods.GET);
						if (descriptionAttribute == null || actionAttribute == null) {
							continue;
						}
						// 有Description属性和类型是GET的Action属性时表示该页面可编辑
						editablePages.Add(new VisualEditablePageInfo(
							plugin.Name,
							descriptionAttribute.Description,
							controllerManager.NormalizePath(actionAttribute.Path)));
					}
				}
			}
			// 更新可编辑的页面Url集合
			EditablePagesSet = new HashSet<string>(editablePages.Select(x => x.Url));
			return editablePages;
		}

		/// <summary>
		/// 判断页面是否可以编辑
		/// </summary>
		public bool IsPageEditable(string url) {
			while (true) {
				var set = EditablePagesSet;
				if (set == null) {
					foreach (var page in GetEditablePages()) { }
					continue;
				}
				return set.Contains(url);
			}
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			EditablePagesSet = null;
		}
	}
}
