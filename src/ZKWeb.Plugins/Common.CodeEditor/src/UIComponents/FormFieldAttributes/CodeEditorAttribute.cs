using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Common.CodeEditor.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 代码编辑器
	/// 字段类型应该是string
	/// </summary>
	public class CodeEditorAttribute : TextAreaFieldAttribute {
		/// <summary>
		/// 语言
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// 自定义配置
		/// </summary>
		public Dictionary<string, object> Config { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>>
		/// <param name="name">字段名称</param>
		/// <param name="rows">行数</param>
		/// <param name="language">语言</param>
		/// <param name="config">自定义配置</param>
		public CodeEditorAttribute(
			string name, int rows, string language, string config = null) :
			base(name, rows, null) {
			Language = language;
			Config = JsonConvert.DeserializeObject<Dictionary<string, object>>(config ?? "{}");
		}
	}
}
