using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
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
		public static void AddDeleteActions(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request,
			Type dataType, string typeName, string batchUrl) {
			// 判断需要添加哪些操作
			bool addBatchDelete = false;
			bool addBatchRecover = false;
			bool addBatchDeleteForever = false;
			if (RecyclableTrait.For(dataType).IsRecyclable) {
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				addBatchDelete = !deleted;
				addBatchRecover = deleted;
				addBatchDeleteForever = deleted;
			} else {
				addBatchDeleteForever = true;
			}
			// 添加批量删除
			typeName = new T(typeName);
			var entityTrait = EntityTrait.For(dataType);
			if (addBatchDelete) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Delete"),
					"fa fa-recycle",
					string.Format(new T("Please select {0} to delete"), typeName),
					new T("Batch Delete"),
					ScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to delete following {0}?"), typeName), "ToString"),
					ScriptStrings.PostConfirmedActionForMultiSelected(
						entityTrait.PrimaryKey, batchUrl + "?action=delete"),
					new { type = "type-danger" });
			}
			// 添加批量恢复
			if (addBatchRecover) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Recover"),
					"fa fa-history",
					string.Format(new T("Please select {0} to recover"), typeName),
					new T("Batch Recover"),
					ScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to recover following {0}?"), typeName), "ToString"),
					ScriptStrings.PostConfirmedActionForMultiSelected(
						entityTrait.PrimaryKey, batchUrl + "?action=recover"));
			}
			// 添加批量永久删除
			if (addBatchDeleteForever) {
				column.AddConfirmActionForMultiChecked(
					new T("Batch Delete Forever"),
					"fa fa-remove",
					string.Format(new T("Please select {0} to delete"), typeName),
					new T("Batch Delete Forever"),
					ScriptStrings.ConfirmMessageTemplateForMultiSelected(
						string.Format(new T("Sure to delete following {0} forever?"), typeName), "ToString"),
					ScriptStrings.PostConfirmedActionForMultiSelected(
						entityTrait.PrimaryKey, batchUrl + "?action=delete_forever"),
					new { type = "type-danger" });
			}
		}

		/// <summary>
		/// 添加删除相关的按钮
		/// 如果数据类型可以回收，则添加批量删除或批量恢复和永久删除
		/// 如果数据类型不可以回收，则添加批量永久删除
		/// 根据增删查改页面的构建器自动生成
		/// </summary>
		/// <typeparam name="TBuilder">构建器的类型</typeparam>
		/// <param name="column">Id列</param>
		/// <param name="request">搜索请求</param>
		public static void AddDeleteActionsForCrudPage<TBuilder>(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			column.AddDeleteActions(request, app.DataType, app.DataTypeName, app.BatchUrl);
		}

		/// <summary>
		/// 对增删查改页面使用的Ajax表格Id列进行标准的设置
		/// 添加以下菜单项
		/// - 删除菜单（如果批量操作Url不是空）
		/// </summary>
		/// <typeparam name="TBuilder">后台应用类型</typeparam>
		/// <param name="column">Id列</param>
		/// <param name="request">搜索请求</param>
		public static void StandardSetupForCrudPage<TBuilder>(
			this AjaxTableIdColumn column, AjaxTableSearchRequest request)
			where TBuilder : class, ICrudPageBuilder, new() {
			var app = new TBuilder();
			if (!string.IsNullOrEmpty(app.BatchUrl)) {
				column.AddDivider();
				column.AddDeleteActionsForCrudPage<TBuilder>(request);
			}
		}
	}
}
