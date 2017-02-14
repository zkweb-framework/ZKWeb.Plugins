using DotLiquid;
using System.IO;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 描画延迟引用的javascript资源
	/// 描画后会清空原有的引用，可以重复调用
	/// </summary>
	/// <example>
	/// {% render_included_js %}
	/// </example>
	public class RenderIncludedJs : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__included_js";
		/// <summary>
		/// Http上下文中的变量名
		/// </summary>
		public const string HttpContextKey = "Common.Base.IncludedJs";

		/// <summary>
		/// 描画引用的内容，并清空原有的引用
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			// 描画模板上下文中的引用
			result.Write(context[Key]);
			context.Environments[0].Remove(Key);
			// 描画Http上下文中的引用
			var includedJs = HttpManager.CurrentContext.GetData<string>(HttpContextKey);
			if (!string.IsNullOrEmpty(includedJs)) {
				result.Write(includedJs);
				HttpManager.CurrentContext.RemoveData(HttpContextKey);
			}
		}
	}
}
