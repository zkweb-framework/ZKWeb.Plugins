using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ImageBrowser", "画像ブラウザ" },
			{ "Provide image browse and upload functions", "画像管理とアップロード機能を提供する" },
			{ "Image", "画像" },
			{ "CustomName", "カスタム名" },
			{ "Please select image file", "画像ファイルを選択してください" },
			{ "ImageUpload", "画像アップロード" },
			{ "ImageBrowse", "画像管理" },
			{ "Upload Successfully", "アップロード成功しました" },
			{ "Use This Image", "この画像を使う" },
			{ "Remove", "削除" },
			{ "Sure to remove this image?", "この画像を削除しますか？" },
			{ "Remove Successfully", "削除成功しました" },
			{ "ImageManage", "画像管理" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
