using System;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格Id列的扩展函数
	/// </summary>
	public static class AjaxTableIdColumnExtensions {
		/// <summary>
		/// 添加删除相关的按钮
		/// 如果数据类型可以回收，则添加批量删除或批量恢复和永久删除
		/// 如果数据类型不可以回收，则添加批量永久删除
		/// </summary>
		/// <param name="column">Id列</param>
		/// <param name="request">搜索请求</param>
		/// <param name="dataType">数据类型</param>
		/// <param name="typeName">类型名称</param>
		/// <param name="batchUrl">批量操作使用的Url</param>
		/// <param name="primaryKey">主键名称，不传入时使用默认</param>
		/// <param name="allowDeleteRecover">是否允许删除恢复，不传入时使用默认</param>
		/// <param name="allowDeleteForever">是否允许永久删除，不传入时使用默认</param>
		public static void AddDeleteActions(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request,
			Type dataType, string typeName, string batchUrl, string primaryKey = null,
			bool? allowDeleteRecover = null, bool? allowDeleteForever = null) {
			// 判断需要添加哪些操作
			bool addBatchDelete = false;
			bool addBatchRecover = false;
			bool addBatchDeleteForever = false;
			if (typeof(IHaveDeleted).GetTypeInfo().IsAssignableFrom(dataType)) {
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				addBatchDelete = (allowDeleteRecover == false) ? false : !deleted;
				addBatchRecover = (allowDeleteRecover == false) ? false : deleted;
				addBatchDeleteForever = (allowDeleteForever == false) ? false : deleted;
			} else {
				addBatchDeleteForever = allowDeleteForever ?? true;
			}
			// 添加批量删除
			typeName = new T(typeName);
			primaryKey = primaryKey ?? nameof(IEntity<Guid>.Id);
			if (addBatchDelete) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Delete"),
					"fa fa-recycle",
					string.Format(new T("Please select {0} to delete"), typeName),
					new T("Batch Delete"),
					BaseScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to delete following {0}?"), typeName), "ToString"),
					BaseScriptStrings.PostConfirmedActionForMultiSelected(
						primaryKey, batchUrl + "?action=delete"),
					new { type = "type-danger" });
			}
			// 添加批量恢复
			if (addBatchRecover) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Recover"),
					"fa fa-history",
					string.Format(new T("Please select {0} to recover"), typeName),
					new T("Batch Recover"),
					BaseScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to recover following {0}?"), typeName), "ToString"),
					BaseScriptStrings.PostConfirmedActionForMultiSelected(
						primaryKey, batchUrl + "?action=recover"));
			}
			// 添加批量永久删除
			if (addBatchDeleteForever) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Delete Forever"),
					"fa fa-remove",
					string.Format(new T("Please select {0} to delete"), typeName),
					new T("Batch Delete Forever"),
					BaseScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to delete following {0} forever?"), typeName), "ToString"),
					BaseScriptStrings.PostConfirmedActionForMultiSelected(
						primaryKey, batchUrl + "?action=delete_forever"),
					new { type = "type-danger" });
			}
		}

		/// <summary>
		/// 添加删除相关的按钮
		/// 如果数据类型可以回收，则添加批量删除或批量恢复和永久删除
		/// 如果数据类型不可以回收，则添加批量永久删除
		/// 根据增删查改页面的控制器自动生成
		/// </summary>
		/// <typeparam name="TBuilder">控制器的类型</typeparam>
		/// <param name="column">Id列</param>
		/// <param name="request">搜索请求</param>
		public static void AddDeleteActionsFor<TCrudController>(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request)
			where TCrudController : class, ICrudController, new() {
			var app = new TCrudController();
			column.AddDeleteActions(request,
				app.EntityType, app.EntityTypeName, app.BatchUrl,
				null, app.AllowDeleteRecover, app.AllowDeleteForever);
		}

		/// <summary>
		/// 对增删查改页面使用的Ajax表格Id列进行标准的设置
		/// 添加以下菜单项
		/// - 删除菜单（如果批量操作Url不是空）
		/// </summary>
		/// <typeparam name="TCrudController">后台应用类型</typeparam>
		/// <param name="column">Id列</param>
		/// <param name="request">搜索请求</param>
		public static void StandardSetupFor<TCrudController>(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request)
			where TCrudController : class, ICrudController, new() {
			var app = new TCrudController();
			if (!string.IsNullOrEmpty(app.BatchUrl)) {
				column.AddDivider();
				column.AddDeleteActionsFor<TCrudController>(request);
			}
		}
	}
}
