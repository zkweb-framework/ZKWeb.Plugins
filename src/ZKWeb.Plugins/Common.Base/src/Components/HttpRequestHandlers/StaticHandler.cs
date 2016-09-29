using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Components.HttpRequestHandlers {
	/// <summary>
	/// 静态文件处理器
	/// 路径规则
	/// /static/{路径}
	///	返回文件
	/// pathManager.GetResourcesPath("static", 路径);
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
			if (path.StartsWith(Prefix)) {
				var fileStorage = Application.Ioc.Resolve<IFileStorage>();
				var fileEntry = fileStorage.GetResourceFile("static", path.Substring(Prefix.Length));
				if (fileEntry.Exists) {
					var ifModifiedSince = context.Request.GetIfModifiedSince();
					new FileEntryResult(fileEntry, ifModifiedSince).WriteResponse(context.Response);
					context.Response.End();
				}
			}
		}
	}
}
