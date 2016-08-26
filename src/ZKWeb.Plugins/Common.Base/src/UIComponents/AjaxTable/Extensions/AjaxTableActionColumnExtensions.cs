using Newtonsoft.Json;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格操作列的扩展函数
	/// </summary>
	public static class AjaxTableActionColumnExtensions {
		/// <summary>
		/// 添加点击事件按钮
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="name">显示名称</param>
		/// <param name="buttonClass">按钮Css类</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="onClick">点击时执行的Javascript代码</param>
		public static void AddButtonForClickEvent(
			this AjaxTableActionColumn column,
			string name, string buttonClass, string iconClass, string onClick) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			column.ActionTemplates.Add(new HtmlString(templateManager.RenderTemplate(
				"common.base/tmpl.ajax_table.action_onclick.html",
				new { name, buttonClass, iconClass, onClick })));
		}

		/// <summary>
		/// 添加点击打开链接的按钮
		/// 可以指定按钮的生成链接的模板
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="name">显示名称</param>
		/// <param name="buttonClass">按钮Css类</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="linkTemplate">链接的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="target">打开目标，默认是_self</param>
		public static void AddButtonForOpenLink(
			this AjaxTableActionColumn column,
			string name, string buttonClass, string iconClass,
			string linkTemplate, string target = null) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			column.ActionTemplates.Add(new HtmlString(templateManager.RenderTemplate(
				"common.base/tmpl.ajax_table.action_link.html", new {
					name,
					buttonClass,
					iconClass,
					linkTemplate = new HtmlString(linkTemplate),
					target = target ?? "_self"
				})));
		}

		/// <summary>
		/// 让最后一个添加的操作只在满足指定条件时显示
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="showIf">显示条件，参数传入row</param>
		public static void MakeLastActionOptional(
			this AjaxTableActionColumn column, string showIf) {
			var html = string.Format("<% if ({0}) {{ %>{1}<% }} %>", showIf, column.ActionTemplates.Last());
			column.ActionTemplates[column.ActionTemplates.Count - 1] = new HtmlString(html);
		}

		/// <summary>
		/// 添加使用模态框弹出指定指定页面的按钮
		/// 链接模板传入按钮所在行的数据
		/// 支持在数据改变后模态框关闭时刷新Ajax表格
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="name">显示名称</param>
		/// <param name="buttonClass">按钮Css类</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="titleTemplate">模态框标题的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="urlTemplate">远程链接的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		public static void AddRemoteModalForBelongedRow(
			this AjaxTableActionColumn column,
			string name, string buttonClass, string iconClass,
			string titleTemplate, string urlTemplate, object dialogParameters = null) {
			column.AddButtonForClickEvent(name, buttonClass, iconClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				var row = table.getBelongedRowData(this);
				row && table.showRemoteModalForRow(row, {0}, {1}, {2});",
				JsonConvert.SerializeObject(titleTemplate),
				JsonConvert.SerializeObject(urlTemplate),
				JsonConvert.SerializeObject(dialogParameters)));
		}

		/// <summary>
		/// 添加对多选框选中的数据进行的需要确认的批量操作菜单项
		/// 确认时会使用模态框
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="name">显示名称</param>
		/// <param name="buttonClass">按钮Css类</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="titleTemplate">标题栏的模板，格式是underscore.js的默认格式，参数传入rows</param>
		/// <param name="messageTemplate">消息内容的模板，格式是underscore.js的默认格式，参数传入rows</param>
		/// <param name="callback">回调，可以使用变量table和rows，result等于true是代表用户点击了确认</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		public static void AddConfirmActionForBelongedRow(
			this AjaxTableActionColumn column,
			string name, string buttonClass, string iconClass,
			string titleTemplate, string messageTemplate,
			string callback, object dialogParameters = null) {
			column.AddButtonForClickEvent(name, buttonClass, iconClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				var rows = [table.getBelongedRowData(this)];
				rows.length && table.showConfirmActionForRows(rows, {0}, {1}, {2}, {3}, function(result) {{ {4} }}, {5});",
				JsonConvert.SerializeObject(new T("Ok")),
				JsonConvert.SerializeObject(new T("Cancel")),
				JsonConvert.SerializeObject(titleTemplate),
				JsonConvert.SerializeObject(messageTemplate),
				callback,
				JsonConvert.SerializeObject(dialogParameters)));
		}
	}
}
