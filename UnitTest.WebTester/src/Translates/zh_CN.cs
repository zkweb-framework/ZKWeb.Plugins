using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "WebTester", "网页单元测试器" },
			{ "Support running unit test from admin panel", "支持在管理员后台运行单元测试" },
			{ "UnitTest", "单元测试" },
			{ "Run", "运行" },
			{ "Assembly", "程序集" },
			{ "Passed", "通过" },
			{ "Skiped", "跳过" },
			{ "Failed", "失败" },
			{ "ErrorMessage", "错误消息" },
			{ "DebugMessage", "除错消息" },
			{ "Start", "开始" },
			{ "StartAll", "全部开始" },
			{ "ResetAll", "全部重置" },
			{ "NotRunning", "未运行" },
			{ "WaitingToRun", "等待运行" },
			{ "Running", "运行中" },
			{ "FinishedRunning", "运行完毕" },
			{ "Getting", "获取中" },
			{ "Request submitted, wait processing", "请求已提交，正在等待处理" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
