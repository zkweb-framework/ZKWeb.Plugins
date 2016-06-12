using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			// TODO: 以下未翻译到其他语言
			{ "PesudoStatic", "伪静态" },
			{ "Pesudo static support", "伪静态支持" },
			{ "PesudoStaticSettings", "伪静态设置" },
			{ "EnablePesudoStatic", "启用伪静态" },
			{ "PesudoStaticExtension", "伪静态扩展名" },
			{ "PesudoStaticParamDelimiter", "伪静态参数分隔符" },
			{ "PesudoStaticPolicy", "伪静态策略" },
			{ "BlackListPolicy", "黑名单策略" },
			{ "WhiteListPolicy", "白名单策略" },
			{ "IncludeUrlPaths", "包含的Url路径" },
			{ "ExcludeUrlPaths", "排除的Url路径" },
			{ "One path per line, only available for whitelist policy", "一行一个，仅在白名单策略下生效" },
			{ "One path per line, only available for blacklist policy", "一行一个，仅在黑名单策略下生效" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
