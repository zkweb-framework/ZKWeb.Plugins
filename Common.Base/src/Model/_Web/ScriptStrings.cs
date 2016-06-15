using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		/// <summary>
		/// 批量操作的确认消息模板
		/// 例
		/// 是否确认要删除以下用户
		/// 用户名
		/// 用户名
		/// </summary>
		/// <param name="message">第一行显示的消息</param>
		/// <param name="member">接下来每行显示的成员名称</param>
		/// <returns></returns>
		public static string ConfirmMessageTemplateForMultiSelected(string message, string member) {
			return string.Format(
				"{0}<%_.each(rows, function(row) {{ print('<br />' + _.escape(row.{1})); }})%>",
				message, member);
		}

		/// <summary>
		/// 在Ajax表格的批量操作确认框已确认时
		/// 使用ajaxTable.postAction提交指定的成员列表到目标地址
		/// </summary>
		/// <param name="member">成员名称</param>
		/// <param name="target">提交到的目标</param>
		/// <returns></returns>
		public static string PostConfirmedActionForMultiSelected(string member, string target) {
			return string.Format(
				"result && table.postAction(_.map(rows, function(row) {{ return row.{0}; }}), {1});",
				member, JsonConvert.SerializeObject(target));
		}

		/// <summary>
		/// 在指定时间后刷新当前页面
		/// </summary>
		/// <param name="milliseconds">指定时间，单位是毫秒</param>
		/// <returns></returns>
		public static string RefreshAfter(int milliseconds) {
			return string.Format(
				"setTimeout(function() {{ location.href = location.href; }}, {0});", milliseconds);
		}

		/// <summary>
		/// 在一定时间后重定向当前页面到指定地址
		/// </summary>
		/// <param name="url">重定向到的地址</param>
		/// <param name="milliseconds">等待时间，单位是毫秒</param>
		/// <returns></returns>
		public static string Redirect(string url, int milliseconds = 1) {
			return string.Format(
				"setTimeout(function() {{ location.href = {0}; }}, {1});",
				JsonConvert.SerializeObject(url), milliseconds);
		}

		/// <summary>
		/// 更新模态框远程页面内容到指定的Url
		/// </summary>
		/// <param name="url">更新到的Url</param>
		/// <returns></returns>
		public static string RedirectModal(string url) {
			return string.Format(
				"$(this).closest('.bootstrap-dialog .remote-contents').attr('href', {0}).trigger('reload');",
				JsonConvert.SerializeObject(url));
		}
	}
}
