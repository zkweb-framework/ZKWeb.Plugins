using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// Html项列表的扩展函数
	/// </summary>
	public static class HtmlItemsExtensions {
		/// <summary>
		/// 添加处理点击事件的按钮
		/// </summary>
		/// <param name="items">html项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标css类</param>
		/// <param name="btnClass">按钮css类</param>
		/// <param name="onClick">点击时执行的Javascript代码</param>
		public static void AddButtonForClickEvent(
			this List<HtmlItem> items, string name, string iconClass, string btnClass, string onClick) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.html_item.btn_onclick.html",
				new { name, iconClass, btnClass, onClick });
			items.Add(new HtmlItem(html));
		}

		/// <summary>
		/// 添加打开链接使用的按钮
		/// </summary>
		/// <param name="items">html项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标css类</param>
		/// <param name="btnClass">按钮css类</param>
		/// <param name="href">链接地址</param>
		/// <param name="target">打开目标</param>
		public static void AddButtonForLink(
			this List<HtmlItem> items, string name, string iconClass, string btnClass,
			string href, string target = "_self") {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.html_item.btn_link.html",
				new { name, iconClass, btnClass, href, target });
			items.Add(new HtmlItem(html));
		}

		/// <summary>
		/// 添加使用模态框弹出指定指定页面的按钮
		/// 支持在数据改变后模态框关闭时刷新Ajax表格
		/// </summary>
		/// <param name="items">html项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="btnClass">按钮css类</param>
		/// <param name="title">模态框标题</param>
		/// <param name="url">远程链接</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		public static void AddRemoteModalForAjaxTable(this List<HtmlItem> items,
			string name, string iconClass, string btnClass,
			string title, string url, object dialogParameters = null) {
			items.AddButtonForClickEvent(name, iconClass, btnClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				table.showRemoteModalForRow(null, {0}, {1}, {2});",
				JsonConvert.SerializeObject(title),
				JsonConvert.SerializeObject(url),
				JsonConvert.SerializeObject(dialogParameters)));
		}
	}
}
