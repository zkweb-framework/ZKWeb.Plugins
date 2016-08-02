using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Captcha.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ZKWeb.Plugins.Common.Captcha.src.Managers {
	/// <summary>
	/// 验证码管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CaptchaManager {
		/// <summary>
		/// 默认的验证码位数
		/// </summary>
		public const int DefaultDigits = 4;
		/// <summary>
		/// 每个字符的宽度
		/// </summary>
		public const int ImageWidthPerChar = 20;
		/// <summary>
		/// 验证码图片的高度
		/// </summary>
		public const int ImageHeight = 32;
		/// <summary>
		/// 图片边距
		/// </summary>
		public const int ImagePadding = 5;
		/// <summary>
		/// 干扰线数量
		/// </summary>
		public const int InterferenceLines = 3;
		/// <summary>
		/// 字符图表的最大拉伸量
		/// </summary>
		public const int CharGraphicMaxPadding = 5;
		/// <summary>
		/// 字符图表的最大旋转量
		/// </summary>
		public const int CharGraphicMaxRotate = 35;
		/// <summary>
		/// 生成验证码后最小保留会话的时间
		/// </summary>
		public const int MakeSessionAliveAtLeast = 30;
		/// <summary>
		/// 保存到会话时使用的键名前缀
		/// </summary>
		public const string SessionItemKeyPrefix = "Common.Captcha.";
		/// <summary>
		/// 当前环境是否支持验证码语音
		/// 注意：
		/// 目前IIS环境中部署会出现调用TTS时线程锁死无法结束的问题，原因无法找到
		/// 也无法找到任何办法预先检测当前环境是否会出现这个问题
		/// 如果碰到此问题请在网站配置中禁用验证码语音
		/// </summary>
		public bool SupportCaptchaAudio { get; set; }
		/// <summary>
		/// 验证码语音参数的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CaptchaAudioPromptCacheTime { get; set; }
		/// <summary>
		/// 验证码语音参数的缓存
		/// </summary>
		protected MemoryCache<string, PromptBuilder> CaptchaAudioPromptCache { get; set; }

		/// <summary>
		/// 验证码管理器
		/// </summary>
		public CaptchaManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			SupportCaptchaAudio = configManager.WebsiteConfig
				.Extra.GetOrDefault<bool?>(ExtraConfigKeys.SupportCaptchaAudio) ?? true;
			CaptchaAudioPromptCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.CaptchaAudioPromptCacheTime, 15));
			CaptchaAudioPromptCache = new MemoryCache<string, PromptBuilder>();
		}

		/// <summary>
		/// 生成验证码并储存验证码到会话中
		/// </summary>
		/// <param name="key">使用的键名</param>
		/// <param name="digits">验证码位数</param>
		/// <param name="chars">使用的字符列表</param>
		/// <returns></returns>
		public virtual Image Generate(string key, int digits = DefaultDigits, string chars = null) {
			// 生成验证码
			var captchaCode = RandomUtils.RandomString(
				digits, chars ?? "23456789ABCDEFGHJKLMNPQRSTUWXYZ");
			var image = new Bitmap(ImageWidthPerChar * digits + ImagePadding * 2, ImageHeight);
			var font = new Font("Arial", ImageWidthPerChar, FontStyle.Bold);
			var rand = RandomUtils.Generator;
			using (var graphic = Graphics.FromImage(image)) {
				// 描画背景
				var backgroundBrush = new SolidBrush(Color.White);
				graphic.FillRectangle(backgroundBrush, new RectangleF(0, 0, image.Width, image.Height));
				// 添加干扰线
				var pen = new Pen(Color.Black);
				for (int x = 0; x < InterferenceLines; ++x) {
					var pointStart = new Point(rand.Next(image.Width), rand.Next(image.Height));
					var pointFinish = new Point(rand.Next(image.Width), rand.Next(image.Height));
					graphic.DrawLine(pen, pointStart, pointFinish);
				}
				// 逐个字符描画，并进行不规则拉伸
				var stringFormat = StringFormat.GenericDefault;
				var randomPadding = new Func<int>(() => rand.Next(CharGraphicMaxPadding));
				var textBrush = new SolidBrush(Color.Black);
				for (int x = 0; x < captchaCode.Length; ++x) {
					var path = new GraphicsPath();
					var rect = new RectangleF(
						ImageWidthPerChar * x + ImagePadding, ImagePadding,
						ImageWidthPerChar, image.Height - ImagePadding);
					var str = captchaCode[x].ToString();
					path.AddString(str, font.FontFamily, (int)font.Style, font.Size, rect, stringFormat);
					// 拉伸
					var points = new PointF[] {
						new PointF(rect.X + randomPadding(), randomPadding()),
						new PointF(rect.X + rect.Width - randomPadding(), randomPadding()),
						new PointF(rect.X + randomPadding(), image.Height - randomPadding()),
						new PointF(rect.X + rect.Width - randomPadding(), image.Height - randomPadding()),
					};
					path.Warp(points, rect);
					// 旋转
					var matrix = new Matrix();
					matrix.RotateAt(rand.Next(-CharGraphicMaxRotate, CharGraphicMaxRotate), rect.Location);
					graphic.Transform = matrix;
					// 描画到图层
					graphic.FillPath(textBrush, path);
				}
			}
			// 储存到会话，会话的过期时间最少是30分钟
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			session.Items[SessionItemKeyPrefix + key] = captchaCode;
			session.SetExpiresAtLeast(TimeSpan.FromMinutes(MakeSessionAliveAtLeast));
			sessionManager.SaveSession();
			// 返回图片对象
			return image;
		}

		/// <summary>
		/// 获取当前验证码，但不删除
		/// 这个函数一般只用于单元测试或语音提示，检查请使用Check函数
		/// </summary>
		/// <param name="key">使用的键名</param>
		/// <returns></returns>
		public virtual string GetWithoutRemove(string key) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			return session.Items.GetOrDefault<string>(SessionItemKeyPrefix + key);
		}

		/// <summary>
		/// 检查验证码是否正确，检查后删除原验证码
		/// </summary>
		/// <param name="key">使用的键名</param>
		/// <param name="actualCode">收到的验证码</param>
		/// <returns></returns>
		public virtual bool Check(string key, string actualCode) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var itemKey = SessionItemKeyPrefix + key;
			var exceptedCode = session.Items.GetOrDefault<string>(itemKey);
			session.Items.Remove(itemKey);
			sessionManager.SaveSession();
			return !string.IsNullOrEmpty(exceptedCode) &&
				!string.IsNullOrEmpty(actualCode) &&
				actualCode.ToLower() == exceptedCode.ToLower();
		}

		/// <summary>
		/// 获取当前验证码的语音提示
		/// </summary>
		/// <param name="key">使用的键名</param>
		/// <returns></returns>
		public virtual MemoryStream GetAudioStream(string key) {
			// 检查是否支持
			if (!SupportCaptchaAudio) {
				throw new NotSupportedException("TTS is not support on this environment");
			}
			// 生成语音到内存
			// 需要使用独立线程否则会提示当前线程不支持异步操作
			// http://stackoverflow.com/questions/10783127/how-to-implement-custom-audio-capcha-in-asp-net
			var captcha = GetWithoutRemove(key) ?? "";
			var stream = new MemoryStream();
			var cultureInfo = Thread.CurrentThread.CurrentCulture;
			var thread = new Thread(() => {
				try {
					// 设置线程语言，语音提示会自动选择对应的语言
					Thread.CurrentThread.CurrentCulture = cultureInfo;
					Thread.CurrentThread.CurrentUICulture = cultureInfo;
					// 构建语音
					// 参数缓存一定时间，防止多次尝试攻击
					var prompt = CaptchaAudioPromptCache.GetOrCreate(captcha, () => {
						var builder = new PromptBuilder();
						var promptRates = new[] { PromptRate.Slow, PromptRate.Medium, PromptRate.Fast };
						var voiceGenders = new[] { VoiceGender.Male, VoiceGender.Female, VoiceGender.Neutral };
						var voiceAges = new[] { VoiceAge.Adult, VoiceAge.Child, VoiceAge.Senior, VoiceAge.Teen };
						foreach (var c in captcha) {
							builder.StartVoice(
								RandomUtils.RandomSelection(voiceGenders),
								RandomUtils.RandomSelection(voiceAges));
							builder.AppendText(c.ToString(), RandomUtils.RandomSelection(promptRates));
							builder.AppendBreak(TimeSpan.FromMilliseconds(RandomUtils.RandomInt(50, 450)));
							builder.EndVoice();
						}
						return builder;
					}, CaptchaAudioPromptCacheTime);
					// 写入语音到数据流
					var synthesizer = new SpeechSynthesizer();
					synthesizer.SetOutputToWaveStream(stream);
					synthesizer.Speak(prompt);
				} catch (Exception e) {
					var logManager = Application.Ioc.Resolve<LogManager>();
					logManager.LogError(e.ToString());
				}
			});
			thread.Start();
			thread.Join();
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}
	}
}
