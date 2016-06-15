using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// Ajax表格Id列的扩展函数
	/// </summary>
	public static class AjaxTableIdColumnExtensions {
		/// <summary>
		/// 添加点击事件菜单项
		/// </summary>
		/// <param name="column">Id列</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="onClick">点击时执行的Javascript代码</param>
		public static void AddItemForClickEvent(
			this AjaxTableIdColumn column, string name, string iconClass, string onClick) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			column.ActionTemplates.Add(new MenuItem(templateManager.RenderTemplate(
				"common.base/tmpl.menu_item.onclick.html",
				new { name, iconClass, onClick })));
		}

		/// <summary>
		/// 添加分割线菜单项
		/// </summary>
		/// <param name="column">Id列</param>
		public static void AddDivider(this AjaxTableIdColumn column) {
			column.ActionTemplates.AddDivider();
		}

		/// <summary>
		/// 添加全选/取消全选菜单项
		/// </summary>
		/// <param name="column">Id列</param>
		public static void AddSelectOrUnselectAll(
			this AjaxTableIdColumn column) {
			column.AddItemForClickEvent(new T("Select/Unselect All"),
				"fa fa-check-square-o",
				"$(this).closestAjaxTable().selectOrUnselectAll()");
		}

		/// <summary>
		/// 添加对多选框选中的数据进行的需要确认的批量操作菜单项
		/// 确认时会使用模态框
		/// </summary>
		/// <param name="column">Id列</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="nonCheckedMessage">没有选中数据时的提示消息</param>
		/// <param name="titleTemplate">标题栏的模板，格式是underscore.js的默认格式，参数传入rows</param>
		/// <param name="messageTemplate">消息内容的模板，格式是underscore.js的默认格式，参数传入rows</param>
		/// <param name="callback">回调，可以使用变量table和rows，result等于true是代表用户点击了确认</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		public static void AddConfirmActionForMultiChecked(
			this AjaxTableIdColumn column, string name, string iconClass,
			string nonCheckedMessage, string titleTemplate, string messageTemplate,
			string callback, object dialogParameters = null) {
			column.AddItemForClickEvent(name, iconClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				var rows = table.getMultiCheckedRowsData();
				if (!rows.length) {{
					$.toast({0});
				}} else {{
					table.showConfirmActionForRows(rows, {1}, {2}, {3}, {4}, function(result) {{ {5} }}, {6});
				}}",
				JsonConvert.SerializeObject(nonCheckedMessage),
				JsonConvert.SerializeObject(new T("Ok")),
				JsonConvert.SerializeObject(new T("Cancel")),
				JsonConvert.SerializeObject(titleTemplate),
				JsonConvert.SerializeObject(messageTemplate),
				callback,
				JsonConvert.SerializeObject(dialogParameters)));
		}
	}
}
