using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "abc", "测试翻译" },
			{ "ZKWeb Default Website", "ZKWeb默认站点" },
			{ "Captcha", "验证码" },
			{ "Click to change captcha image", "点击更换验证码图片" },
			{ "Please enter captcha", "请填写验证码" },
			{ "Incorrect captcha", "验证码错误，请重新填写" },
			{ "Length of {0} must be {1}", "{0}的长度必须是{1}" },
			{ "Length of {0} must between {1} and {2}", "{0}的长度必须在{1}和{2}之间" },
			{ "HomePage", "首页" },
			{ "How to edit this page", "怎样编辑这个页面" },
			{ "Use Plugin", "使用插件" },
			{ "Copy Common.Base/templates/common.base/index.html to Your.Plugin/templates/common.base/index.html then edit it.",
				"复制Common.Base/templates/common.base/index.html到你的插件/templates/common.base/index.html然后编辑" },
			{ "Use Diy", "使用Diy" },
			{ "Diy is not ready yet.", "Diy功能尚未完成" }
		};
		
		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
