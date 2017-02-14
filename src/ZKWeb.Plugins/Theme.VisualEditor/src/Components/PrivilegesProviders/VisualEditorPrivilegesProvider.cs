using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeProviders;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.PrivilegeProviders {
	/// <summary>
	/// 提供可视化编辑权限
	/// </summary>
	[ExportMany]
	public class VisualEditorPrivilegesProvider : IPrivilegesProvider {
		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPrivileges() {
			yield return "VisualEditor:VisualEditor";
		}
	}
}
