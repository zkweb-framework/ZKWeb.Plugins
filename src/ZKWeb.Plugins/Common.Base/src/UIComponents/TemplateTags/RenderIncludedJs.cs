using DotLiquid;
using System.IO;

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
		/// 描画引用的内容，并清空原有的引用
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			result.Write(context[Key]);
			context.Environments[0].Remove(Key);
		}
	}
}
