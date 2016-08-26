using DotLiquid;
using System.IO;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 描画额外的meta标签
	/// 描画后会清空原有的内容，可以重复调用
	/// </summary>
	/// <example>
	/// {% render_extra_metadata %}
	/// </example>
	public class RenderExtraMetadata : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__extra_metadata";

		/// <summary>
		/// 描画额外的meta标签，并清空原有的内容
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			result.Write(context[Key]);
			context.Environments[0].Remove(Key);
		}
	}
}
