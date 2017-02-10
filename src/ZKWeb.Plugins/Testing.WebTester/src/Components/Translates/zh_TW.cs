using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Testing.WebTester.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "WebTester", "網頁單元測試器" },
			{ "Support running tests from admin panel", "支持在管理員後台運行測試" },
			{ "UnitTest", "單元測試" },
			{ "Run", "運行" },
			{ "Assembly", "程序集" },
			{ "Passed", "通過" },
			{ "Skipped", "跳過" },
			{ "Failed", "失敗" },
			{ "ErrorMessage", "錯誤消息" },
			{ "DebugMessage", "除錯消息" },
			{ "Start", "開始" },
			{ "StartAll", "全部開始" },
			{ "ResetAll", "全部重置" },
			{ "NotRunning", "未運行" },
			{ "WaitingToRun", "等待運行" },
			{ "Running", "運行中" },
			{ "FinishedRunning", "運行完畢" },
			{ "Getting", "獲取中" },
			{ "Request submitted, wait processing", "請求已提交，正在等待處理" },
			{ "Function Test", "功能測試" },
			{ "Run tests", "運行測試" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
