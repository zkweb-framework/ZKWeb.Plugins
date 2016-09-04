using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Components.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Logistics", "物流" },
			{ "LogisticsManage", "物流管理" },
			{ "Logistics management for ec site", "商城使用的物流管理功能" },
			{ "LogisticsPriceRules", "運費規則" },
			{ "Logistics cost is determined by the following settings, match order is from top to bottom",
				"物流的運費由以下設置決定，匹配順序是從上到下" },
			{ "LogisticsType", "物流類型" },
			{ "Express", "快遞" },
			{ "SurfaceMail", "物流" },
			{ "FirstHeavyUnit(g)", "首重(克)" },
			{ "FirstHeavyCost", "首重費用" },
			{ "ContinuedHeavyUnit(g)", "續重(克)" },
			{ "ContinuedHeavyCost", "續重費用" },
			{ "Disabled", "已禁用" },
			{ "Selected logistics does not exist", "選擇的物流不存在" },
			{ "Selected logistics is not available for this region", "選擇的物流不支持您的地區，請選擇其他物流" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
