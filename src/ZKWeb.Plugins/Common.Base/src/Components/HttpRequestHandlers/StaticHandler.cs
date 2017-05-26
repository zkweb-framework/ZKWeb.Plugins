using ZKWeb.Storage;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Components.HttpRequestHandlers {
	/// <summary>
	/// 静态文件处理器
	/// 路径规则
	/// /static/{路径}
	///	返回文件
	/// fileStorage.GetResourceFile("static", 路径);
	/// 文件不存在时抛出404例外
	/// </summary>
	[ExportMany, SingletonReuse]
	public class StaticHandler : IHttpRequestHandler {
		/// <summary>
		/// 路径前缀
		/// </summary>
		public const string Prefix = "/static/";

		/// <summary>
		/// 处理请求
		/// </summary>
		public void OnRequest() {
			var context = HttpManager.CurrentContext;
			var path = context.Request.Path;
			if (!path.StartsWith(Prefix)) {
				return;
			}
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var subPath = HttpUtils.UrlDecode(path.Substring(Prefix.Length));
			if (string.IsNullOrEmpty(subPath)) {
				return;
			}
			var fileEntry = fileStorage.GetResourceFile("static", subPath);
			if (!fileEntry.Exists) {
				return;
			}
			var ifModifiedSince = context.Request.GetIfModifiedSince();
			var bytesRange = context.Request.GetBytesRange();
			var result = new FileEntryResult(fileEntry, ifModifiedSince, bytesRange);
			result.WriteResponse(context.Response);
			context.Response.End();
		}
	}
}
