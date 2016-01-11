using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// Ajax表格操作列的扩展函数
	/// </summary>
	public static class AjaxTableActionColumnExtensions {
		/// <summary>
		/// 添加查看按钮
		/// 点击后弹出编辑数据的模态框
		/// 各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TApp">后台应用的类型</typeparam>
		public static void AddEditActionForAdminApp<TApp>(
			this AjaxTableActionColumn column,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null, object dialogParameters = null)
			where TApp : class, IAdminAppBuilder, new() {
			var app = new TApp();
			column.AddRemoteModalForBelongedRow(
				name ?? new T("View"),
				buttonClass ?? "btn btn-xs default",
				iconClass ?? "fa fa-edit",
				titleTemplate ?? new T("Edit " + app.GetDataType().Name),
				urlTemplate ?? (app.EditUrl + "?id=<%-row.Id%>"),
				dialogParameters);
		}
	}
}
