using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// Ajax表格构建器的扩展函数
	/// </summary>
	public static class AjaxTableBuilderExtensions {
		/// <summary>
		/// 对后台应用使用的表格构建器进行标准的设置
		/// 添加以下菜单项
		/// - 编辑菜单（如果编辑Url不是空）
		/// - 添加菜单（如果添加Url不是空）
		/// </summary>
		/// <typeparam name="TApp"></typeparam>
		/// <param name="table"></param>
		public static void StandardSetupForAdminApp<TApp>(this AjaxTableBuilder table)
			where TApp : class, IAdminAppBuilder, new() {
			var app = new TApp();
			var addDividerOnce = new Lazy<Action>(
				() => { table.MenuItems.AddDivider(); return () => { }; });
			if (!string.IsNullOrEmpty(app.EditUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddEditActionForAdminApp<TApp>();
			}
			if (!string.IsNullOrEmpty(app.AddUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddAddActionForAdminApp<TApp>();
			}
		}
	}
}
