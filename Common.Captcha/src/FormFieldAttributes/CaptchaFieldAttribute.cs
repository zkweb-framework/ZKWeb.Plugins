using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Captcha.src.FormFieldAttributes {
	/// <summary>
	/// 图片验证码
	/// </summary>
	public class CaptchaFieldAttribute : TextBoxFieldAttribute, IFormFieldParseFromEnv {
		/// <summary>
		/// 用于区分各个验证码的键值
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="key">用于区分各个验证码的键值</param>
		/// <param name="placeHolder">预置文本</param>
		public CaptchaFieldAttribute(string name, string key, string placeHolder = null)
			: base(name, placeHolder) {
			Key = key;
		}
	}
}
