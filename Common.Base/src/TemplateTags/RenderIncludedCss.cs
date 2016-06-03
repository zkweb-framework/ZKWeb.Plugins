using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 描画延迟引用的css资源
	/// 例子
	/// {% render_included_css %}
	/// 描画后会清空原有的引用，可以重复调用
	/// </summary>
	public class RenderIncludedCss : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__included_css";

		/// <summary>
		/// 描画引用的内容，并清空原有的引用
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			result.Write(context[Key]);
			context.Environments[0].Remove(Key);
		}
	}
}
