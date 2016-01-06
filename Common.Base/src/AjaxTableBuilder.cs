using DotLiquid;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// Ajax表格构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// 例子
	///		...
	/// </summary>
	[ExportMany]
	public class AjaxTableBuilder : ILiquidizable {
		/// <summary>
		/// 表格菜单Id的后缀，表格菜单Id = 表格Id + 后缀
		/// </summary>
		public const string MenuIdSuffix = "Menu";
		/// <summary>
		/// 表格Id，必填
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 获取表格数据的url，必填
		/// </summary>
		public string Target { get; set; }
		/// <summary>
		/// 使用的模板，不设置时使用默认
		/// </summary>
		public string Template { get; set; }
		/// <summary>
		/// 右键菜单项的Html列表，默认包含刷新和全屏
		/// </summary>
		public List<string> MenuItems { get; private set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableBuilder() {
			MenuItems = new List<string>();
			MenuItems.Add("<li><a class='refresh'><i class='fa fa-refresh'></i>Refresh</a></li>");
			MenuItems.Add("<li><a class='fullscreen'><i class='fa fa-arrows-alt'></i>Fullscreen</a></li>");
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(ToString());
		}

		/// <summary>
		/// 描画Ajax表格
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderAjaxTable(HtmlTextWriter html) {
			html.AddAttribute("id", Id);
			html.AddAttribute("class", "ajax-table");
			if (MenuItems.Any()) {
				html.AddAttribute("data-toggle", "context");
				html.AddAttribute("data-target", "#" + Id + MenuIdSuffix);
			}
			html.AddAttribute("ajax-table-target", Target);
			if (!string.IsNullOrEmpty(Template)) {
				html.AddAttribute("ajax-table-template", Template);
			}
			html.RenderBeginTag("div");
			html.RenderEndTag();
		}

		/// <summary>
		/// 描画Ajax表格菜单
		/// </summary>
		/// <param name="html"></param>
		protected virtual void RenderAjaxTableMenu(HtmlTextWriter html) {
			if (!MenuItems.Any()) {
				return;
			}
			html.AddAttribute("id", Id + MenuIdSuffix);
			html.AddAttribute("class", "ajax-table-menu");
			html.AddAttribute("ajax-table", "#" + Id);
			html.RenderBeginTag("div");
			html.AddAttribute("class", "dropdown-menu");
			html.AddAttribute("role", "menu");
			html.RenderBeginTag("ul");
			foreach (var item in MenuItems) {
				html.WriteLine(item);
			}
			html.RenderEndTag(); // ul
			html.RenderEndTag(); // div
		}

		/// <summary>
		/// 获取表格html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Target)) {
				throw new ArgumentNullException("Id and Target can't be empty");
			}
			var html = new HtmlTextWriter(new StringWriter());
			RenderAjaxTable(html);
			RenderAjaxTableMenu(html);
			return html.InnerWriter.ToString();
		}
	}
}
