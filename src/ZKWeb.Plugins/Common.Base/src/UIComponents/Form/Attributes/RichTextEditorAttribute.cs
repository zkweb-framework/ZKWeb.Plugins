using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 富文本编辑器的属性
	/// 这个属性需要其他插件实现，请注意引用实现的插件
	/// </summary>
	public class RichTextEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 自定义配置
		/// </summary>
		public Dictionary<string, object> Config { get; set; }
		/// <summary>
		/// 图片管理器地址，指定时可以启用图片上传功能
		/// </summary>
		public string ImageBrowserUrl {
			get { return Config.GetOrDefault<string>("imageBrowserUrl"); }
			set { Config["imageBrowserUrl"] = value; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="config">自定义配置，格式是Json</param>
		public RichTextEditorAttribute(string name, string config = null) {
			Name = name;
			Config = JsonConvert.DeserializeObject<Dictionary<string, object>>(config ?? "{}");
		}
	}
}
