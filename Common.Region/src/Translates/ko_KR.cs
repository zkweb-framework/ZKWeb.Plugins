using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Region", "지방" },
			{ "Provide regions and related functions", "지역 목록 및 관련 기능" },
			{ "RegionSettings", "장소" },
			{ "DefaultCountry", "기본 국가/관리" },
			{ "DisplayCountryDropdown", "디스플레이 국가 / 관리 지역 드롭 다운 상자" },
			{ "CN", "중화 인민 공화국" },
			{ "US", "미국" },
			{ "FR", "프랑스" },
			{ "GB", "영어" },
			{ "DE", "독일" },
			{ "JP", "일본의" },
			{ "KR", "대한민국" },
			{ "ES", "스페인의 왕국" },
			{ "TH", "태국" },
			{ "TW", "대만" },
			{ "HK", "홍콩 특별 행정구" },
			{ "RU", "러시아" },
			{ "IT", "이탈리아어 공화국" },
			{ "GR", "그리스 공화국" },
			{ "AE", "아랍 에미리트" },
			{ "PL", "폴란드의 공화국" },
			{ "CZ", "체코 공화국" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
