using System;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.PesudoStatic.Wrappers;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.UrlFilters;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.HttpRequestHandlers {
	/// <summary>
	/// 伪静态使用的Http请求处理器
	/// 完整规则请查看"PesudoStaticUrlFilter"的说明
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PesudoStaticHandlerWrapper : IHttpRequestHandlerWrapper {
		/// <summary>
		/// 包装请求
		/// </summary>
		/// <param name="action">请求函数</param>
		/// <returns></returns>
		public Action WrapHandlerAction(Action action) {
			return () => {
				// 关闭伪静态时不处理
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<PesudoStaticSettings>();
				if (!settings.EnablePesudoStatic) {
					action();
					return;
				}
				// 判断路径是否以伪静态后缀结尾
				var context = HttpManager.CurrentContext;
				var request = context.Request;
				var path = request.Path;
				if (!path.EndsWith(settings.PesudoStaticExtension)) {
					action();
					return;
				}
				var query = request.GetQueryValues().ToDictionary(p => p.First, p => p.Second);
				// 解析伪静态路径
				// - 无参数时: {原始路径}{后缀名}
				// - 只有id参数时: {原始路径}{分隔符}{id}{后缀名}
				// - 包含其他参数时: {原始路径}{分隔符}({参数名称}{分隔符}{参数值}...){后缀名}
				path = path.Substring(0, path.Length - settings.PesudoStaticExtension.Length);
				var lastPartIndex = path.LastIndexOf('/');
				if (lastPartIndex < 0) {
					// 实际不会出现路径中没有"/"的情况
					action();
					return;
				}
				var lastPart = path.Substring(lastPartIndex + 1);
				var parts = lastPart.Split(settings.PesudoStaticParamDelimiter);
				if (parts.Length > 1) {
					// 有参数时需要解析原始路径和设置参数
					path = path.Substring(0, lastPartIndex + 1) + parts[0];
					if (parts.Length == 2) {
						// 只有id参数
						if (!query.ContainsKey(PesudoStaticUrlFilter.IdParameterName)) {
							query[PesudoStaticUrlFilter.IdParameterName] = new[] { parts[1] };
						}
					} else if (parts.Length % 2 == 1) {
						// 包含其他参数
						// 不需要处理解码，构建时会排除需要解码的参数
						for (int i = 2; i < parts.Length; i += 2) {
							var key = parts[i - 1];
							var value = parts[i];
							if (!query.ContainsKey(key)) {
								query[key] = new[] { value };
							}
						}
					} else {
						// 参数数量不正确时不处理
						action();
						return;
					}
				}
				// 尝试以解析后的Url进行处理
				var queryString = (query.Count > 0) ? "?" + HttpUtils.BuildQueryString(query) : "";
				try {
					// 重载当前的http上下文，尝试进行处理
					using (HttpManager.OverrideContext(
						new PesudoStaticHttpContext(context, path, queryString))) {
						action();
					};
					// 处理解析后的url成功
					return;
				} catch (HttpException e) {
					if (e.StatusCode != 404) {
						throw;
					}
				}
				// 使用原来的Url进行处理
				action();
			};
		}
	}
}
