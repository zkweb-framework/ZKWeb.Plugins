using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.GenericTag.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Tag", "유니버설 레이블" },
			{ "Generic tag management", "일반 관리자 태그" },
			{ "TagManage", "라벨 관리" },
			{ "DefaultTag", "기본 탭" },
			{ "Try to access tag that type not matched", "레이블과 일치하지 않는 작업의 유형을보십시오" },
			{ "DisplayOrder", "표시 순서" },
			{ "Order from small to large", "정렬" },
			{ "Tag", "꼬리표" },
			{ "Tags", "꼬리표" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
