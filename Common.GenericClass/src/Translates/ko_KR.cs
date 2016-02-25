using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.GenericClass.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Generic Class", "일반적인 분류" },
			{ "Generic class/catalog management", "경영 일반 분류" },
			{ "ClassManage", "카테고리 관리" },
			{ "DefaultClass", "기본 카테고리" },
			{ "Try to access class that type not matched", " 조작 형 불일치 분류를 시도" },
			{ "DisplayOrder", "표시 순서" },
			{ "Order from small to large", "정렬" },
			{ "ParentClass", "하위 제목" },
			{ "Add Top Level Class", "상위 카테고리 추가" },
			{ "Add Same Level Class", "형제 분류 추가" },
			{ "Add Child Class", "하위 카테고리를 추가" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
