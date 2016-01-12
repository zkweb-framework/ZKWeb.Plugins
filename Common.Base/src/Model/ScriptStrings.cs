using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 常用脚本字符串
	/// </summary>
	public static class ScriptStrings {
		/// <summary>
		/// 通知Ajax表格数据已更新
		/// 调用后在模态框关闭时表格会自动刷新
		/// </summary>
		public const string AjaxtableUpdatedFromModal = (
			"$(this).closest('.bootstrap-dialog').trigger('updated.ajaxTable');");
		/// <summary>
		/// 触发模态框远程页面内容的更新事件
		/// 一般在提交后，需要刷新当前模态框内容时使用
		/// </summary>
		public const string ReloadModal = (
			"$(this).closest('.bootstrap-dialog .remote-contents').trigger('reload');");
		/// <summary>
		/// 关闭模态框
		/// </summary>
		public const string CloseModal = (
			"$(this).closest('.bootstrap-dialog').modal('hide');");
		/// <summary>
		/// 通知Ajax表格数据已更新，并关闭模态框
		/// </summary>
		public const string AjaxtableUpdatedAndCloseModal = AjaxtableUpdatedFromModal + CloseModal;
	}
}
