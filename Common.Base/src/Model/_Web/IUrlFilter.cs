using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 全局Url过滤器
	/// </summary>
	public interface IUrlFilter {
		/// <summary>
		/// 过滤Url
		/// </summary>
		/// <param name="url">url</param>
		void Filter(ref string url);
	}
}
