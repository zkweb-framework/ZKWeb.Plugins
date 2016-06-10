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
		/// 对增删查改页面使用的表格构建器进行标准的设置
		/// 添加以下菜单项
		/// - 编辑菜单（如果编辑Url不是空）
		/// - 添加菜单（如果添加Url不是空）
		/// </summary>
		/// <typeparam name="TBuilder">构建器的类型</typeparam>
		/// <param name="table">Ajax表格</param>
		public static void StandardSetupForCrudPage<TBuilder>(this AjaxTableBuilder table)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			var addDividerOnce = new Lazy<Action>(
				() => { table.MenuItems.AddDivider(); return () => { }; });
			if (!string.IsNullOrEmpty(app.EditUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddEditActionForCrudPage<TBuilder>();
			}
			if (!string.IsNullOrEmpty(app.AddUrl)) {
				addDividerOnce.Value();
				table.MenuItems.AddAddActionForCrudPage<TBuilder>();
			}
		}
	}
}
