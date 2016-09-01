using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Testing.WebTester.src.Components.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "WebTester", "ウェブユニットテスター" },
			{ "Support running unit test from admin panel", "管理者パネルからユニットテストを実行する機能" },
			{ "UnitTest", "ユニットテスト" },
			{ "Run", "実行" },
			{ "Assembly", "アセンブリ" },
			{ "Passed", "通過" },
			{ "Skipped", "スキップ" },
			{ "Failed", "失敗" },
			{ "ErrorMessage", "エラーメッセージ" },
			{ "DebugMessage", "デバッグメッセージ" },
			{ "Start", "開始" },
			{ "StartAll", "全部開始する" },
			{ "ResetAll", "全部リセットする" },
			{ "NotRunning", "なし" },
			{ "WaitingToRun", "実行待ち" },
			{ "Running", "実行中" },
			{ "FinishedRunning", "実行完了" },
			{ "Getting", "取得中" },
			{ "Request submitted, wait processing", "リクエストを送信しました、処理を待っています" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
