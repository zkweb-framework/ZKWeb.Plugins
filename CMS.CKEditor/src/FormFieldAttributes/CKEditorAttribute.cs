using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.CMS.CKEditor.src.FormFieldAttributes {
	/// <summary>
	/// CKEditor编辑器的属性
	/// </summary>
	public class CKEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 传给CKEditor的配置
		/// </summary>
		public Dictionary<string, object> Config { get; set; }
		/// <summary>
		/// 图片上传类目，指定时可以启用图片上传功能
		/// </summary>
		public string ImageUploadCategory {
			get { return Config.GetOrDefault<string>("imageUploadCategory"); }
			set { Config["imageUploadCategory"] = value; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="config">传给CKEditor的配置，格式是Json</param>
		public CKEditorAttribute(string name, string config = null) {
			Name = name;
			Config = JsonConvert.DeserializeObject<Dictionary<string, object>>(config ?? "{}");
		}
	}
}
