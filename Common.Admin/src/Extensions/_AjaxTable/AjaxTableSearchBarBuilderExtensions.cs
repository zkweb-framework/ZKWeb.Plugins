using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
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
		/// <typeparam name="TBuilder">后台应用类型</typeparam>
		/// <param name="searchBar">搜索栏构建器</param>
		/// <param name="keywordPlaceHolder">关键词的预留文本，传入后会自动翻译，可传入原文</param>
		public static void StandardSetupForCrudPage<TBuilder>(
			this AjaxTableSearchBarBuilder searchBar, string keywordPlaceHolder)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			searchBar.KeywordPlaceHolder = new T(keywordPlaceHolder);
			var addDividerOnce = new Lazy<Action>(
				() => { searchBar.MenuItems.AddDivider(); return () => { }; });
			if (RecyclableTrait.For(app.DataType).IsRecyclable) {
				addDividerOnce.Value();
				searchBar.MenuItems.AddRecycleBin();
			}
			if (!string.IsNullOrEmpty(app.AddUrl)) {
				addDividerOnce.Value();
				searchBar.MenuItems.AddAddActionForCrudPage<TBuilder>();
				searchBar.BeforeItems.AddAddActionForCrudPage<TBuilder>();
			}
		}
	}
}
