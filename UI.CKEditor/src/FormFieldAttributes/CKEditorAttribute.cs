using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.UI.CKEditor.src.FormFieldAttributes {
	/// <summary>
	/// CKEditor编辑器的属性
	/// </summary>
	public class CKEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 传给CKEditor的配置，格式是Json
		/// </summary>
		public string Config { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="config">传给CKEditor的配置，格式是Json</param>
		public CKEditorAttribute(string name, string config = "null") {
			Name = name;
			Config = config;
		}
	}
}
