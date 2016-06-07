using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// Ajax表格搜索栏构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// 例子
	///		var searchBar = Application.Ioc.Resolve[AjaxTableSearchBarBuilder]();
	///		searchBar.TableId = "TestList";
	///		searchBar.Conditions.Add(new FormField(new TextBoxFieldAttribute("TestCondition")));
	///		return new TemplateResult("test_table.html", new { table, searchBar });
	/// </summary>
	[ExportMany]
	public class AjaxTableSearchBarBuilder : ILiquidizable {
		/// <summary>
		/// 表格Id，必填
		/// </summary>
		public string TableId { get; set; }
		/// <summary>
		/// 关键字的预留文本
		/// </summary>
		public string KeywordPlaceHolder { get; set; }
		/// <summary>
		/// 菜单项列表，默认包含刷新，全屏，操作和页面设置
		/// </summary>
		public List<MenuItem> MenuItems { get; protected set; }
		/// <summary>
		/// 高级搜索框的条件项
		/// </summary>
		public List<FormField> Conditions { get; protected set; }
		/// <summary>
		/// 显示在搜索框之前的项
		/// </summary>
		public List<HtmlItem> BeforeItems { get; protected set; }
		/// <summary>
		/// 显示在搜索框之后的项
		/// </summary>
		public List<HtmlItem> AfterItems { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableSearchBarBuilder() {
			MenuItems = new List<MenuItem>();
			Conditions = new List<FormField>();
			BeforeItems = new List<HtmlItem>();
			AfterItems = new List<HtmlItem>();
			MenuItems.AddRefresh();
			MenuItems.AddFullscreen();
			MenuItems.AddDivider();
			MenuItems.AddTableOperations();
			MenuItems.AddPaginationSettings();
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(ToString());
		}

		/// <summary>
		/// 获取表格html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (string.IsNullOrEmpty(TableId)) {
				throw new ArgumentNullException("TableId can't be empty");
			}
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.ajax_table.search_bar.html", new {
				tableId = TableId,
				menuItems = MenuItems,
				placeholder = new T(KeywordPlaceHolder ?? "Please enter keyword"),
				conditions = Conditions,
				beforeItems = BeforeItems,
				afterItems = AfterItems
			});
			return html;
		}
	}
}
