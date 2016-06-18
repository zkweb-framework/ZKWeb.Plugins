using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>() {
			{ "PesudoStatic", "偽靜態" },
			{ "Pesudo static support", "偽靜態支持" },
			{ "PesudoStaticSettings", "偽靜態設置" },
			{ "EnablePesudoStatic", "啟用偽靜態" },
			{ "PesudoStaticExtension", "偽靜態擴展名" },
			{ "PesudoStaticParamDelimiter", "偽靜態參數分隔符" },
			{ "PesudoStaticPolicy", "偽靜態策略" },
			{ "BlackListPolicy", "黑名單策略" },
			{ "WhiteListPolicy", "白名單策略" },
			{ "IncludeUrlPaths", "包含的Url路徑" },
			{ "ExcludeUrlPaths", "排除的Url路徑" },
			{ "One path per line, only available for whitelist policy", "壹行壹個，僅在白名單策略下生效" },
			{ "One path per line, only available for blacklist policy", "壹行壹個，僅在黑名單策略下生效" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
