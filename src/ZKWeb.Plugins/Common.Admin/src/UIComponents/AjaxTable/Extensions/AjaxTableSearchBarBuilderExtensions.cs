using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格搜索栏构建器的扩展函数
	/// </summary>
	public static class AjaxTableSearchBarBuilderExtensions {
		/// <summary>
		/// 对增删查改页面使用的搜索栏构建器进行标准的设置
		/// 添加以下菜单项
		/// - 回收站（如果数据支持回收）
		/// - 添加菜单（如果添加Url不是空）
		/// 添加以下按钮
		/// - 添加按钮（如果添加Url不是空）
		/// </summary>
		/// <typeparam name="TCrudController">控制器的类型</typeparam>
		/// <param name="searchBar">搜索栏构建器</param>
		/// <param name="keywordPlaceHolder">关键词的预留文本，传入后会自动翻译，可传入原文</param>
		public static void StandardSetupFor<TCrudController>(
			this AjaxTableSearchBarBuilder searchBar, string keywordPlaceHolder)
			where TCrudController : class, ICrudController, new() {
			var app = new TCrudController();
			searchBar.KeywordPlaceHolder = new T(keywordPlaceHolder);
			var addDividerOnce = new Lazy<Action>(
				() => { searchBar.MenuItems.AddDivider(); return () => { }; });
			if (app.AllowDeleteRecover) {
				addDividerOnce.Value();
				searchBar.MenuItems.AddRecycleBin();
			}
			if (!string.IsNullOrEmpty(app.AddUrl)) {
				addDividerOnce.Value();
				searchBar.MenuItems.AddAddActionFor<TCrudController>();
				searchBar.BeforeItems.AddAddActionFor<TCrudController>();
			}
		}
	}
}
