using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;

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
				buttonClass ?? "btn btn-xs btn-info",
				iconClass ?? "fa fa-edit",
				titleTemplate ?? string.Format(new T("Edit {0}"), new T(typeName)),
				urlTemplate ?? (editUrl + "?id=<%-row.Id%>"),
				dialogParameters);
		}

		/// <summary>
		/// 添加查看按钮
		/// 点击后弹出编辑数据的模态框
		/// 根据增删查改页面的构建器自动生成，各个参数如不指定则使用默认值
		/// </summary>
		/// <typeparam name="TBuilder">后台应用的类型</typeparam>
		public static void AddEditActionForCrudPage<TBuilder>(
			this AjaxTableActionColumn column,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null, object dialogParameters = null)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			column.AddEditAction(app.DataTypeName, app.EditUrl,
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
		/// <param name="primaryKey">数据Id保存的名称，不指定时使用默认值</param>
		/// <param name="dialogParameters">弹出框的参数，不指定时使用默认值</param>
		public static void AddDeleteAction(
			this AjaxTableActionColumn column, string typeName, string deleteUrl,
			string name = null, string buttonClass = null, string iconClass = null,
			string titleTemplate = null, string urlTemplate = null,
			string primaryKey = null, object dialogParameters = null) {
			primaryKey = primaryKey ?? EntityTrait.For<object>().PrimaryKey;
			column.AddConfirmActionForBelongedRow(
				name ?? new T("Delete"),
				buttonClass ?? "btn btn-xs btn-danger",
				iconClass ?? "fa fa-remove",
				titleTemplate ?? string.Format(new T("Delete {0}"), new T(typeName)),
				ScriptStrings.ConfirmMessageTemplateForMultiSelected(
					string.Format(new T("Sure to delete following {0}?"), new T(typeName)), "ToString"),
				ScriptStrings.PostConfirmedActionForMultiSelected(primaryKey, deleteUrl),
				dialogParameters);
		}

		/// <summary>
		/// 对增删查改页面使用的Ajax表格操作列进行标准的设置
		/// 添加以下按钮
		/// - 查看按钮（如果编辑Url不是空）
		/// </summary>
		/// <typeparam name="TBuilder">构建器的类型</typeparam>
		/// <param name="column">操作列</param>
		/// <param name="request">搜索请求</param>
		public static void StandardSetupForCrudPage<TBuilder>(
			this AjaxTableActionColumn column, AjaxTableSearchRequest request)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			if (!string.IsNullOrEmpty(app.EditUrl)) {
				column.AddEditActionForCrudPage<TBuilder>();
			}
		}
	}
}
