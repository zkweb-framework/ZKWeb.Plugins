using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;
using ZKWeb.Server;
using ZKWeb.Web;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.HttpRequestHandlers {
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
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var filePath = pathManager.GetResourceFullPath("static", path.Substring(Prefix.Length));
				if (filePath != null) {
					var ifModifiedSince =
						context.Request.Headers["If-Modified-Since"].ConvertOrDefault<DateTime?>();
					new FileResult(filePath, ifModifiedSince).WriteResponse(context.Response);
					context.Response.End();
				}
			}
		}
	}
}
