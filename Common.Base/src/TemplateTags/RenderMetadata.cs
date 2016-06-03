using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 描画附加的内容
	/// 用于描画head标签中的meta标签
	/// 例子
	/// {% render_metadata %}
	/// 描画后会清空原有的内容，可以重复调用
	/// </summary>
	public class RenderMetadata : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__head_metadata";

		/// <summary>
		/// 描画附加的内容，并清空原有的内容
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			result.Write(context[Key]);
			context.Environments[0].Remove(Key);
		}
	}
}
