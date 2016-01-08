using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 后台应用构建器的接口
	/// 用于绕开泛型获取
	/// </summary>
	public interface IAdminAppBuilder {
		/// <summary>
		/// 网站启动时的处理
		/// </summary>
		void OnWebsiteStart();
	}
}
