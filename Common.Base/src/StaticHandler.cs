using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 静态文件处理器
	/// 路径规则
	///		/static/{路径}
	///	返回文件
	///		pathManager.GetResourcesPath("static", 路径);
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
			var context = HttpContext.Current;
			var path = context.Request.Path;
			if (path.StartsWith(Prefix)) {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var filePath = pathManager.GetResourceFullPath("static", path.Substring(Prefix.Length));
				if (filePath != null) {
					// 文件存在时写到http回应中
					new FileResult(filePath).WriteResponse(context.Response);
					context.Response.End();
				}
			}
		}
	}
}
