namespace ZKWeb.Plugins.Common.Captcha.src.Components.ExtraConfigKeys {
	/// <summary>
	/// 网站附加配置中使用的键
	/// </summary>
	public class CaptchaExtraConfigKeys {
		/// <summary>
		/// 是否支持验证码语音
		/// </summary>
		public const string SupportCaptchaAudio = "Common.Captcha.SupportCaptchaAudio";
		/// <summary>
		/// 验证码语音参数的缓存时间，单位是秒
		/// </summary>
		public const string CaptchaAudioPromptCacheTime = "Common.Captcha.CaptchaAudioPromptCacheTime";
	}
}
