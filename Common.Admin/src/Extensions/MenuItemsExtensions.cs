﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// 菜单项列表的扩展函数
	/// </summary>
	public static class MenuItemsExtensions {
		/// <summary>
		/// 添加添加按钮
		/// 点击后弹出添加数据的模态框
		/// 各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TApp">后台应用的类型</typeparam>
		public static void AddAddActionForAdminApp<TApp>(
			this List<MenuItem> items, string name = null, string iconClass = null,
			string title = null, string url = null, object dialogParameters = null)
			where TApp : class, IAdminAppBuilder, new() {
			var app = new TApp();
			var defaultName = new T("Add " + app.GetDataType().Name);
			items.AddRemoteModalForAjaxTable(
				name ?? defaultName,
				iconClass ?? "fa fa-plus",
				title ?? defaultName,
				url ?? app.AddUrl,
				dialogParameters);
		}
	}
}
