using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ImageBrowser", "이미지 브라우저" },
			{ "Provide image browse and upload functions", "이미지 검색 및 업로드 기능을 제공합니다" },
			{ "Image", "영상" },
			{ "CustomName", "사용자 이름" },
			{ "Please select image file", "이미지 파일을 선택하세요" },
			{ "ImageUpload", "이미지 업로드" },
			{ "ImageBrowse", "이미지 브라우저" },
			{ "Upload Successfully", "성공적으로 업로드" },
			{ "Use This Image", "이 이미지를 사용하여" },
			{ "Remove", "없애다" },
			{ "Sure to remove this image?", "이 이미지를 제거해야합니다？" },
			{ "Remove Successfully", "성공적으로 제거" },
			{ "ImageManage", "이미지 관리" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
