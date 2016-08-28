using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;

namespace ZKWeb.Plugins.Common.Captcha.src.UIComponents.Form.Attributes {
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
		public CaptchaFieldAttribute(string name, string key, string placeHolder = null) :
			base(name, placeHolder) {
			Key = key;
		}
	}
}
