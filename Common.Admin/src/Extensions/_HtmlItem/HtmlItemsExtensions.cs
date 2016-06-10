using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// Html项列表的扩展函数
	/// </summary>
	public static class HtmlItemsExtensions {
		/// <summary>
		/// 添加添加按钮
		/// 点击后弹出添加数据的模态框
		/// </summary>
		/// <param name="items">html项列表</param>
		/// <param name="typeName">类型名称</param>
		/// <param name="addUrl">添加使用的Url</param>
		/// <param name="name">显示名称，不指定时使用默认值</param>
		/// <param name="iconClass">图标的Css类，不指定时使用默认值</param>
		/// <param name="btnClass">按钮的Css类，不指定时使用默认值</param>
		/// <param name="title">标题，不指定时使用默认值</param>
		/// <param name="url">添加使用的Url，和addUrl是同一个参数</param>
		/// <param name="dialogParameters">弹出框的参数，不指定时使用默认值</param>
		public static void AddAddAction(
			this List<HtmlItem> items, string typeName, string addUrl,
			string name = null, string iconClass = null, string btnClass = null,
			string title = null, string url = null, object dialogParameters = null) {
			var defaultName = string.Format(new T("Add {0}"), new T(typeName));
			items.AddRemoteModalForAjaxTable(
				name ?? defaultName,
				iconClass ?? "fa fa-plus",
				btnClass ?? "btn btn-default",
				title ?? defaultName,
				url ?? addUrl,
				dialogParameters);
		}

		/// <summary>
		/// 添加添加按钮
		/// 点击后弹出添加数据的模态框
		/// 根据增删查改页面的构建器自动生成，各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TBuilder">构建器的类型</typeparam>
		public static void AddAddActionForCrudPage<TBuilder>(
			this List<HtmlItem> items, string name = null,
			string iconClass = null, string btnClass = null,
			string title = null, string url = null, object dialogParameters = null)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			items.AddAddAction(app.DataTypeName, app.AddUrl,
				name, iconClass, btnClass, title, url, dialogParameters);
		}
	}
}
