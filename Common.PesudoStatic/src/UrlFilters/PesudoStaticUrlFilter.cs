using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.PesudoStatic.src.Config;
using ZKWeb.Plugins.Common.PesudoStatic.src.Model;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.UrlFilters {
	/// <summary>
	/// 伪静态使用的Url过滤器
	/// <example>
	/// 规则
	/// - 如果路径是空，跳过
	/// - 如果路径是"/"，跳过
	/// - 如果路径以"#"开始，跳过
	/// - 如果路径包含"://"，跳过
	/// - 如果路径已经有后缀名，跳过
	/// - 如果路径的最后一段是空，跳过（以"/"结束时）
	/// - 如果伪静态未启用，跳过
	/// - 如果路径的最后一段包含分隔符，跳过
	/// - 判断伪静态策略
	///   - 黑名单策略: 如果路径在排除路径中，跳过
	///   - 白名单策略: 如果路径不在包括路径中，跳过
	/// - 路径规则
	///   - 无参数时: {原始路径}{后缀名}
	///   - 只有id参数时: {原始路径}{分隔符}{id}{后缀名}
	///   - 包含其他参数时: {原始路径}{分隔符}({参数名称}{分隔符}{参数值}...){后缀名}
	///   - 如果路径中的参数或它的值包含分隔符，该参数不参与到伪静态路径中
	///   - 如果路径中的参数或它的值经过url编码后和原值不一致，该参数不参与到伪静态路径中
	///     - 这样做的原因是服务器会允许参数中有经过编码后的特殊字符
	///     - 但不允许路径中有经过编码后的特殊字符，或双重编码
	/// 
	/// 示例
	/// / => /
	/// /user/login => /user/login.html
	/// /example/view?id=1 => /example/view-1.html
	/// /example/list?tag=123&class=158 => /example/list-tag-123-class-158.html
	/// /example/list?key=1-1&class=123 => /example/list-class-123.html?key=1-1
	/// </example>
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PesudoStaticUrlFilter : IUrlFilter {
		/// <summary>
		/// Id参数的名称
		/// </summary>
		public const string IdParameterName = "id";

		/// <summary>
		/// 过滤Url
		/// </summary>
		public void Filter(ref string url) {
			// 如果路径是空，跳过
			// 如果路径是"/"，跳过
			// 如果路径以"#"开始，跳过
			// 如果路径包含"://"，跳过
			var pathAndQuery = url.Split(new[] { '?' }, 2);
			var path = pathAndQuery[0];
			var query = (pathAndQuery.Length > 1) ? pathAndQuery[1] : "";
			if (string.IsNullOrEmpty(path) || path == "/" ||
				path[0] == '#' || path.IndexOf("://") >= 0) {
				return;
			}
			// 如果路径已经有后缀名，跳过
			// 如果路径的最后一段是空，跳过（以"/"结束时）
			// 如果伪静态未启用，跳过
			// 如果路径的最后一段包含分隔符，跳过
			var lastPartIndex = path.LastIndexOf('/');
			var lastPart = lastPartIndex >= 0 ? path.Substring(lastPartIndex + 1) : path;
			if (string.IsNullOrEmpty(lastPart) || lastPart.Contains('.')) {
				return;
			}
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<PesudoStaticSettings>();
			if (!settings.EnablePesudoStatic ||
				lastPart.Contains(settings.PesudoStaticParamDelimiter)) {
				return;
			}
			// 判断伪静态策略
			// - 黑名单策略: 如果路径在排除路径中，跳过
			// - 白名单策略: 如果路径不在包括路径中，跳过
			if (settings.PesudoStaticPolicy == PesudoStaticPolicies.BlackListPolicy &&
				settings.ExcludeUrlPaths.Contains(path)) {
				return;
			} else if (settings.PesudoStaticPolicy == PesudoStaticPolicies.WhiteListPolicy &&
				!settings.IncludeUrlPaths.Contains(path)) {
				return;
			}
			// 路径规则
			// - 无参数时: {原始路径}{后缀名}
			// - 只有id参数时: {原始路径}{分隔符}{id}{后缀名}
			// - 包含其他参数时: {原始路径}{分隔符}({参数名称}{分隔符}{参数值}...){后缀名}
			// - 如果路径中的参数或它的值包含分隔符，该参数不参与到伪静态路径中
			// - 如果路径中的参数或它的值经过url编码后和原值不一致，该参数不参与到伪静态路径中
			if (string.IsNullOrEmpty(query)) {
				url = path + settings.PesudoStaticExtension;
				return;
			}
			var parameters = HttpUtils.ParseQueryString(query);
			if (parameters.Count == 1) {
				var idParameter = parameters[IdParameterName];
				if (idParameter != null && idParameter.Count == 1) {
					url = (new StringBuilder()
						.Append(path)
						.Append(settings.PesudoStaticParamDelimiter)
						.Append(idParameter[0])
						.Append(settings.PesudoStaticExtension).ToString());
					return;
				}
			}
			var urlBuilder = new StringBuilder();
			var excludedParameters = HttpUtils.ParseQueryString("");
			urlBuilder.Append(path);
			foreach (var key in parameters.Keys) {
				var values = parameters[key];
				if (key.Contains(settings.PesudoStaticParamDelimiter) ||
					values.Any(t => t.Contains(settings.PesudoStaticParamDelimiter)) ||
					key != HttpUtils.UrlEncode(key) ||
					values.Any(t => t != HttpUtils.UrlEncode(t))) {
					excludedParameters[key] = values;
					continue;
				}
				foreach (var value in values) {
					urlBuilder.Append(settings.PesudoStaticParamDelimiter);
					urlBuilder.Append(key);
					urlBuilder.Append(settings.PesudoStaticParamDelimiter);
					urlBuilder.Append(value);
				}
			}
			urlBuilder.Append(settings.PesudoStaticExtension);
			if (excludedParameters.Count > 0) {
				urlBuilder.Append('?');
				urlBuilder.Append(HttpUtils.BuildQueryString(excludedParameters));
			}
			url = urlBuilder.ToString();
		}
	}
}
