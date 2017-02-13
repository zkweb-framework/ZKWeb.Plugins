using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.HttpRequestHandlers {
	/// <summary>
	/// 可视化编辑器处理器
	/// 路径规则
	/// /visual_editor/{路径}?参数
	/// 显示原页面并注入可视化编辑器的脚本和样式文件
	/// </summary>
	[ExportMany, SingletonReuse]
	public class VisualEditorPreHandler : IHttpRequestPreHandler {
		/// <summary>
		/// 路径前缀
		/// </summary>
		public const string Prefix = "/visual_editor/";
		/// <summary>
		/// 重载Http上下文的对象保存在Http上下文数据的键
		/// </summary>
		public const string OverridedContextKey = "VisualEditor.OverridedContext";

		/// <summary>
		/// 处理请求
		/// </summary>
		public void OnRequest() {
			var context = HttpManager.CurrentContext;
			var path = context.Request.Path;
			if (!path.StartsWith(Prefix)) {
				return;
			}
			var realPath = "/" + path.Substring(Prefix.Length);
		}
	}
}
