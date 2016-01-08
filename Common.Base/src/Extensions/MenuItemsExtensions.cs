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
	/// 菜单项列表的扩展函数
	/// </summary>
	public static class MenuItemsExtensions {
		/// <summary>
		/// 添加刷新项
		/// 可用于
		///		ajax表格，要求在.ajax-table-menu中
		/// </summary>
		/// <param name="items"></param>
		public static void AddRefresh(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.refresh.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加全屏项
		/// 可用于
		///		全屏所在的.portlet
		/// </summary>
		/// <param name="items"></param>
		public static void AddFullscreen(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.fullscreen.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加分割线
		/// </summary>
		/// <param name="items"></param>
		public static void AddDivider(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.divider.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加对表格的操作
		/// 包含导出xls，打印
		/// </summary>
		/// <param name="items"></param>
		public static void AddTableOperations(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.table_operations.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加每页数量设置
		/// </summary>
		/// <param name="items"></param>
		/// <param name="pageSizes">默认[5, 25, 50, 100, 500]</param>
		public static void AddPaginationSettings(this List<MenuItem> items, int[] pageSizes = null) {
			pageSizes = pageSizes ?? new[] { 5, 25, 50, 100, 500 };
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.menu_item.pagination_settings.html", new { pageSizes });
			items.Add(new MenuItem(html));
		}
	}
}
