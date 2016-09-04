using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.HtmlItems.Extensions {
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
		/// 根据增删查改页面的控制器自动生成，各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TCrudController">控制器的类型</typeparam>
		public static void AddAddActionFor<TCrudController>(
			this List<HtmlItem> items, string name = null,
			string iconClass = null, string btnClass = null,
			string title = null, string url = null, object dialogParameters = null)
			where TCrudController : class, ICrudController, new() {
			var app = new TCrudController();
			items.AddAddAction(app.EntityTypeName, app.AddUrl,
				name, iconClass, btnClass, title, url, dialogParameters);
		}
	}
}
