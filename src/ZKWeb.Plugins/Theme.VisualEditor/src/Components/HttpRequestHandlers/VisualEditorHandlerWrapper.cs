using System;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Theme.VisualEditor.src.Components.HttpContextWrappers;
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
	public class VisualEditorHandlerWrapper : IHttpRequestHandlerWrapper {
		/// <summary>
		/// 路径前缀
		/// </summary>
		public const string Prefix = "/visual_editor/";
		/// <summary>
		/// 重载Http上下文的对象保存在Http上下文数据的键
		/// </summary>
		public const string OverridedContextKey = "VisualEditor.OverridedContext";

		/// <summary>
		/// 包装请求函数
		/// </summary>
		public Action WrapHandlerAction(Action action) {
			return () => {
				var context = HttpManager.CurrentContext;
				var path = context.Request.Path;
				if (!path.StartsWith(Prefix)) {
					action();
					return;
				}
				// 描画原路径的内容同时嵌入可视化编辑的css和js
				var realPath = "/" + path.Substring(Prefix.Length);
				context.IncludeCssLater("/static/template.visualeditor.css/components.css");
				context.IncludeJsLater("/static/template.visualeditor.js/components.min.js");
				using (HttpManager.OverrideContext(
					new VisualEditorHttpContext(context, realPath))) {
					action();
				}
			};
		}
	}
}
