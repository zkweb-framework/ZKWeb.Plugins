using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ImageBrowser", "圖片瀏覽器" },
			{ "Provide image browse and upload functions", "提供圖片瀏覽和上傳的功能" },
			{ "Image", "圖片" },
			{ "CustomName", "自定義名稱" },
			{ "Please select image file", "請選擇圖片文件" },
			{ "ImageUpload", "圖片上傳" },
			{ "ImageBrowse", "圖片瀏覽" },
			{ "Upload Successfully", "上傳成功" },
			{ "Use This Image", "使用這張圖片" },
			{ "Remove", "刪除" },
			{ "Sure to remove this image?", "確認刪除這張圖片嗎？" },
			{ "Remove Successfully", "刪除成功" },
			{ "ImageManage", "圖片管理" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
