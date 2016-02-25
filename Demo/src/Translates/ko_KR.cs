using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Demo.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Demo", "표시" },
			{ "Trun website into demo mode", "데모 모드를 활성화 사이트" },
			{ "Edit or delete demo account is not allowed", "편집하거나 데모 계정을 삭제할 수 없습니다" },
			{ "Background changed by demo plugin", "배경 색상 위젯은 데모 수정되었습니다" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
