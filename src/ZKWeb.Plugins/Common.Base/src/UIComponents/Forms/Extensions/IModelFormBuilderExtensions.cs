using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions {
	/// <summary>
	/// 模型表单构建器的扩展函数
	/// </summary>
	public static class IModelFormBuilderExtensions {
		/// <summary>
		/// 返回保存成功，关掉模态框并且通知Ajax表格更新数据
		/// </summary>
		/// <param name="form">表单</param>
		/// <returns></returns>
		public static object SaveSuccessAndCloseModal(this IModelFormBuilder form) {
			return new {
				message = new T("Saved Successfully"),
				script = BaseScriptStrings.AjaxtableUpdatedAndCloseModal
			};
		}

		/// <summary>
		/// 返回保存成功，并在一定时间后刷新当前页面
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="milliseconds">刷新延迟</param>
		/// <returns></returns>
		public static object SaveSuccessAndRefreshPage(this IModelFormBuilder form, int milliseconds) {
			return new {
				message = new T("Saved Successfully"),
				script = BaseScriptStrings.RefreshAfter(milliseconds)
			};
		}
	}
}
