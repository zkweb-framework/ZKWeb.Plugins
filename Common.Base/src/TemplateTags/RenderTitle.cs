using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 描画当前网站标题
	/// 例子
	/// {% render_title %}
	/// </summary>
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
