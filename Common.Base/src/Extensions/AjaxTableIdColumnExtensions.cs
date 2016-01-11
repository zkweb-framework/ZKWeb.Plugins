using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;

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
	}
}
