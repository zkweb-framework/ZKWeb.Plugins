using System;
using System.Collections.Generic;
using ZKWeb.Cache;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.VisualEditablePagesProviders.Interfaces;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
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
		/// 初始化
		/// </summary>
		public DefaultVisualEditablePagesProvider() {

		}

		/// <summary>
		/// 获取可编辑的页面列表
		/// </summary>
		public IEnumerable<VisualEditablePageInfo> GetEditablePages() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 判断页面是否可以编辑
		/// </summary>
		public bool IsPageEditable(string url) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
		}
	}
}
