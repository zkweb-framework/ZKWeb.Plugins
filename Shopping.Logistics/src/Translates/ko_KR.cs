using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Logistics", "물류" },
			{ "LogisticsManage", "물류 관리" },
			{ "Logistics management for ec site", "쇼핑 사이트 물류 관리" },
			{ "LogisticsPriceRules", "출하 규칙" },
			{ "Logistics cost is determined by the following settings, match order is from top to bottom",
				"물류화물은 위에서 아래로 순서와 일치 다음 설정에 따라 결정됩니다" },
			{ "LogisticsType", "물류 유형" },
			{ "Express", "특급 배달" },
			{ "SurfaceMail", "물류" },
			{ "FirstHeavyUnit(g)", "먼저 무거운 단위(그램)" },
			{ "FirstHeavyCost", "먼저 중공업 비용" },
			{ "ContinuedHeavyUnit(g)", "계속 무거운 단위(그램)" },
			{ "ContinuedHeavyCost", "계속 무거운 비용" },
			{ "Disabled", "장애인" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
