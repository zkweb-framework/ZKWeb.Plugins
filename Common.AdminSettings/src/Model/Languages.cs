using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Model {
	/// <summary>
	/// 语言
	/// </summary>
	public enum Languages {
		/// <summary>
		/// 中文
		/// </summary>
		[Description("zh-CN")]
		Chinese,
		/// <summary>
		/// 英语
		/// </summary>
		[Description("en-US")]
		English,
		/// <summary>
		/// 法语
		/// </summary>
		[Description("fr-FR")]
		French,
		/// <summary>
		/// 德语
		/// </summary>
		[Description("de-DE")]
		German,
		/// <summary>
		/// 日语
		/// </summary>
		[Description("ja-JP")]
		Japanese,
		/// <summary>
		/// 韩语
		/// </summary>
		[Description("ko-KR")]
		Korean,
		/// <summary>
		/// 西班牙语
		/// </summary>
		[Description("es-ES")]
		Spanish,
		/// <summary>
		/// 泰语
		/// </summary>
		[Description("th-TH")]
		Thai,
		/// <summary>
		/// 繁体中文
		/// </summary>
		[Description("zh-TW")]
		TraditionalChinese,
		/// <summary>
		/// 俄语
		/// </summary>
		[Description("ru-RU")]
		Russian,
		/// <summary>
		/// 意大利语
		/// </summary>
		[Description("it-IT")]
		Italian,
		/// <summary>
		/// 希腊语
		/// </summary>
		[Description("el-GR")]
		Greek,
		/// <summary>
		/// 阿拉伯语
		/// </summary>
		[Description("ar-DZ")]
		Arabic,
		/// <summary>
		/// 波兰语
		/// </summary>
		[Description("pl-PL")]
		Polish,
		/// <summary>
		/// 捷克语
		/// </summary>
		[Description("cs-CZ")]
		Czech,
	}
}
