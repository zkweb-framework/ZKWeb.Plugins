using System;
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
	/// Ajax表格操作列的扩展函数
	/// </summary>
	public static class AjaxTableActionColumnExtensions {
		/// <summary>
		/// 添加查看按钮
		/// 点击后弹出编辑数据的模态框
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="typeName">类型名称</param>
		/// <param name="editUrl">编辑使用的Url</param>
		/// <param name="name">名称，不指定时使用默认值</param>
		/// <param name="buttonClass">按钮的Css类，不指定时使用默认值</param>
		/// <param name="iconClass">图标的Css类，不指定时使用默认值</param>
		/// <param name="titleTemplate">标题的模板，格式是underscore.js的格式，参数传入row</param>
		/// <param name="urlTemplate">编辑Url的模板，格式是underscore.js的格式，参数传入row</param>
		/// <param name="dialogParameters">弹出框的参数，不指定时使用默认值</param>
		public static void AddEditAction(
			this AjaxTableActionColumn column, string typeName, string editUrl,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null, object dialogParameters = null) {
			column.AddRemoteModalForBelongedRow(
				name ?? new T("View"),
				buttonClass ?? "btn btn-xs default",
				iconClass ?? "fa fa-edit",
				titleTemplate ?? string.Format(new T("Edit {0}"), new T(typeName)),
				urlTemplate ?? (editUrl + "?id=<%-row.Id%>"),
				dialogParameters);
		}

		/// <summary>
		/// 添加查看按钮
		/// 点击后弹出编辑数据的模态框
		/// 根据后台应用自动生成，各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TApp">后台应用的类型</typeparam>
		public static void AddEditActionForAdminApp<TApp>(
			this AjaxTableActionColumn column,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null, object dialogParameters = null)
			where TApp : class, IAdminAppBuilder, new() {
			var app = new TApp();
			column.AddEditAction(app.TypeName, app.EditUrl,
				name, buttonClass, iconClass, titleTemplate, urlTemplate, dialogParameters);
		}

		/// <summary>
		/// 添加删除按钮
		/// 点击后弹出确认框，确认后把json=[数据Id]提交到删除url
		/// </summary>
		/// <param name="column">操作列</param>
		/// <param name="typeName">类型名称</param>
		/// <param name="deleteUrl">删除使用的Url</param>
		/// <param name="name">名称，不指定时使用默认值</param>
		/// <param name="buttonClass">按钮的Css类，不指定时使用默认值</param>
		/// <param name="iconClass">图标的Css类，不指定时使用默认值</param>
		/// <param name="titleTemplate">标题的模板，格式是underscore.js的格式，参数传入rows</param>
		/// <param name="urlTemplate">编辑Url的模板，格式是underscore.js的格式，参数传入rows</param>
		/// <param name="dialogParameters">弹出框的参数，不指定时使用默认值</param>
		public static void AddDeleteAction(
			this AjaxTableActionColumn column, string typeName, string deleteUrl,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null, object dialogParameters = null) {
			column.AddConfirmActionForBelongedRow(
				name ?? new T("Delete"),
				buttonClass ?? "btn btn-xs btn-danger",
				iconClass ?? "fa fa-remove",
				titleTemplate ?? string.Format(new T("Delete {0}"), new T(typeName)),
				ScriptStrings.ConfirmMessageTemplateForMultiSelected(
					string.Format(new T("Sure to delete following {0}?"), new T(typeName)), "ToString"),
				ScriptStrings.PostConfirmedActionForMultiSelected("Id", deleteUrl),
				dialogParameters);
		}
	}
}
