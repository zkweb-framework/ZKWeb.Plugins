using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Region", "区域" },
			{ "Provide regions and related functions", "提供区域列表和相关的功能" },
			{ "RegionSettings", "地区设置" },
			{ "DefaultCountry", "默认国家/行政区" },
			{ "CN", "中华人民共和国" },
			{ "US", "美利坚合众国" },
			{ "FR", "法国" },
			{ "DE", "德国" },
			{ "JP", "日本国" },
			{ "KR", "大韩民国" },
			{ "ES", "西班牙王国" },
			{ "TH", "泰国" },
			{ "TW", "台湾" },
			{ "HK", "香港特别行政区" },
			{ "RU", "俄罗斯联邦" },
			{ "IT", "意大利共和国" },
			{ "GR", "希腊共和国" },
			{ "AE", "阿拉伯联合酋长国" },
			{ "PL", "波兰共和国" },
			{ "CZ", "捷克共和国" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
