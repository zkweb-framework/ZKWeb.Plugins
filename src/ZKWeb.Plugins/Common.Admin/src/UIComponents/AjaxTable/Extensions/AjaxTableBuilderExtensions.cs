using System;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格构建器的扩展函数
	/// </summary>
	public static class AjaxTableBuilderExtensions {
		/// <summary>
		/// 对增删查改页面使用的表格构建器进行标准的设置
		/// 添加以下菜单项
		/// - 编辑菜单（如果编辑Url不是空）
		/// - 添加菜单（如果添加Url不是空）
		/// </summary>
		/// <typeparam name="TCrudController">构建器的类型</typeparam>
		/// <param name="table">Ajax表格</param>
		public static void StandardSetupFor<TCrudController>(this AjaxTableBuilder table)
			where TCrudController : class, ICrudController, new() {
			var app = new TCrudController();
			var addDividerOnce = new Lazy<Action>(
				() => { table.MenuItems.AddDivider(); return () => { }; });
			if (!string.IsNullOrEmpty(app.EditUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddEditActionFor<TCrudController>();
			}
			if (!string.IsNullOrEmpty(app.AddUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddAddActionFor<TCrudController>();
			}
		}
	}
}
