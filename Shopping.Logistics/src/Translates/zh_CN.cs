using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Logistics", "物流" },
			{ "LogisticsManage", "物流管理" },
			{ "Logistics management for ec site", "商城使用的物流管理功能" },
			{ "LogisticsPriceRules", "运费规则" },
			{ "Logistics cost is determined by the following settings, match order is from top to bottom",
				"物流的运费由以下设置决定，匹配顺序是从上到下" },
			{ "LogisticsType", "物流类型" },
			{ "Express", "快递" },
			{ "SurfaceMail", "物流" },
			{ "FirstHeavyUnit(g)", "首重(克)" },
			{ "FirstHeavyCost", "首重费用" },
			{ "ContinuedHeavyUnit(g)", "续重(克)" },
			{ "ContinuedHeavyCost", "续重费用" },
			{ "Disabled", "已禁用" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
