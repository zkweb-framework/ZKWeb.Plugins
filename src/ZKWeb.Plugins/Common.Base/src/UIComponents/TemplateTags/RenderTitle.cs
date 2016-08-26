using DotLiquid;
using System.IO;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 描画当前网站标题
	/// </summary>
	/// <example>
	/// {% render_title %}
	/// </example>
	public class RenderTitle : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__title";

		/// <summary>
		/// 描画当前网站标题
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var title = context[Key];
			result.Write(string.Format("<title>{0}</title>", title));
		}
	}
}
