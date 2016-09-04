using System.Collections.Generic;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;
using ZKWebStandard.Web.Wrappers;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.PesudoStatic.Wrappers {
	/// <summary>
	/// 伪静态使用的Http请求
	/// 用于重载路径相关的成员
	/// </summary>
	internal class PesudoStaticHttpRequest : HttpRequestWrapper {
		public string ParsedPath { get; protected set; }
		public string ParsedQueryString { get; protected set; }
		public IDictionary<string, IList<string>> ParsedQueryValues { get; set; }
		public override string Path { get { return ParsedPath; } }
		public override string QueryString { get { return ParsedQueryString; } }
		public override IList<string> GetQueryValue(string key) {
			return ParsedQueryValues.GetOrDefault(key);
		}
		public override IEnumerable<Pair<string, IList<string>>> GetQueryValues() {
			foreach (var pair in ParsedQueryValues) {
				yield return Pair.Create(pair.Key, pair.Value);
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalRequest">原始的http请求</param>
		/// <param name="path">解析后的请求路径</param>
		/// <param name="queryString">解析后的请求参数</param>
		public PesudoStaticHttpRequest(
			IHttpRequest originalRequest, string path, string queryString) :
			base(originalRequest) {
			ParsedPath = path;
			ParsedQueryString = queryString;
			ParsedQueryValues = HttpUtils.ParseQueryString(queryString);
		}
	}
}
