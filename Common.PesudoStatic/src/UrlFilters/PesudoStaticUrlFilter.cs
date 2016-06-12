using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.UrlFilters {
	/// <summary>
	/// 伪静态使用的Url过滤器
	/// 规则
	/// TODO: 待编写
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PesudoStaticUrlFilter : IUrlFilter {
		/// <summary>
		/// 过滤Url
		/// </summary>
		public void Filter(ref string url) {
		}
	}
}
