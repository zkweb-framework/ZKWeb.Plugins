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
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Logistics", "物流" },
			{ "LogisticsManage", "物流管理" },
			{ "Logistics management for ec site", "オンラインショッピングサイトが使う物流管理機能" },
			{ "LogisticsPriceRules", "送料ルール" },
			{ "Logistics cost is determined by the following settings, match order is from top to bottom",
				"送料は以下のルールをもって計算されます，照合順番は上から下へ" },
			{ "LogisticsType", "物流タイプ" },
			{ "Express", "速達郵便" },
			{ "SurfaceMail", "船便" },
			{ "FirstHeavyUnit(g)", "第一段階の重さ(グラム)" },
			{ "FirstHeavyCost", "第一段階の費用" },
			{ "ContinuedHeavyUnit(g)", "続く段階ごとの重さ(グラム)" },
			{ "ContinuedHeavyCost", "続く段階ごとの費用" },
			{ "Disabled", "禁止" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
