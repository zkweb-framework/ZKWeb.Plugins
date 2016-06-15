using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.PesudoStatic.src.Config;
using ZKWeb.Plugins.Common.PesudoStatic.src.Model;
using ZKWeb.Plugins.Common.PesudoStatic.src.UrlFilters;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.HttpRequestHandlers {
	/// <summary>
	/// 伪静态使用的Http请求处理器
	/// 完整规则请查看"PesudoStaticUrlFilter"的说明
	/// </summary>
	[ExportMany]
	public class PesudoStaticHandler : IHttpRequestHandler {
		/// <summary>
		/// 处理请求
		/// </summary>
		public void OnRequest() {
			// 关闭伪静态时不处理
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<PesudoStaticSettings>();
			if (!settings.EnablePesudoStatic) {
				return;
			}
			// 判断路径是否以伪静态后缀结尾
			var context = HttpManager.CurrentContext;
			var request = context.Request;
			var url = request.Url;
			var path = url.AbsolutePath;
			if (!path.EndsWith(settings.PesudoStaticExtension)) {
				return;
			}
			var query = HttpUtility.ParseQueryString(request.Url.Query);
			// 解析伪静态路径
			// - 无参数时: {原始路径}{后缀名}
			// - 只有id参数时: {原始路径}{分隔符}{id}{后缀名}
			// - 包含其他参数时: {原始路径}{分隔符}({参数名称}{分隔符}{参数值}...){后缀名}
			path = path.Substring(0, path.Length - settings.PesudoStaticExtension.Length);
			var lastPartIndex = path.LastIndexOf('/');
			if (lastPartIndex < 0) {
				return; // 实际不会出现路径中没有"/"的情况
			}
			var lastPart = path.Substring(lastPartIndex + 1);
			var parts = lastPart.Split(settings.PesudoStaticParamDelimiter);
			if (parts.Length > 1) {
				// 有参数时需要解析原始路径和设置参数
				path = path.Substring(0, lastPartIndex + 1) + parts[0];
				if (parts.Length == 2) {
					// 只有id参数
					if (query[PesudoStaticUrlFilter.IdParameterName] == null) {
						query[PesudoStaticUrlFilter.IdParameterName] = parts[1];
					}
				} else if (parts.Length % 2 == 1) {
					// 包含其他参数
					// 不需要处理解码，构建时会排除需要解码的参数
					for (int i = 2; i < parts.Length; i += 2) {
						var key = parts[i - 1];
						var value = parts[i];
						if (query[key] == null) {
							query[key] = value;
						}
					}
				} else {
					// 参数数量不正确时不处理
					return;
				}
			}
			// 构建解析后的url
			var overrideUrlBuilder = new StringBuilder();
			overrideUrlBuilder.Append(url.Scheme);
			overrideUrlBuilder.Append("://");
			overrideUrlBuilder.Append(url.Authority);
			overrideUrlBuilder.Append(path);
			if (query.Count > 0) {
				overrideUrlBuilder.Append('?');
				overrideUrlBuilder.Append(query.ToString());
			}
			// 重载当前的http上下文，然后调用其它处理器处理
			var overrideUrl = new Uri(overrideUrlBuilder.ToString());
			var overrideRequest = new PesudoStaticHttpRequest(request, overrideUrl);
			var overrideContext = new PesudoStaticHttpContext(context, overrideRequest);
			using (HttpContextUtils.OverrideContext(overrideContext)) {
				var handlers = Application.Ioc.ResolveMany<IHttpRequestHandler>().Reverse();
				foreach (var handler in handlers) {
					if (handler is PesudoStaticHandler) {
						continue;
					}
					handler.OnRequest();
				}
			}
			// 没有处理器成功处理解析后的url，把解析前的url交给其他处理器处理
		}
	}
}
