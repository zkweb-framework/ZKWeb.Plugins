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
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ImageBrowser", "图片浏览器" },
			{ "Provide image browse and upload functions", "提供图片浏览和上传的功能" },
			{ "Image", "图片" },
			{ "CustomName", "自定义名称" },
			{ "Please select image file", "请选择图片文件" },
			{ "ImageUpload", "图片上传" },
			{ "ImageBrowse", "图片浏览" },
			{ "Upload Successfully", "上传成功" },
			{ "Use This Image", "使用这张图片" },
			{ "Remove", "删除" },
			{ "Sure to remove this image?", "确认删除这张图片吗？" },
			{ "Remove Successfully", "删除成功" },
			{ "ImageManage", "图片管理" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
