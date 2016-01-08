using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 后台应用构建器
	/// 支持自动生成列表和增删查改页面（Scaffold，半自动）
	/// 例子
	///		...
	/// </summary>
	/// <typeparam name="T">管理的数据类型</typeparam>
	[ExportMany]
	public abstract class AdminAppBuilder<T> : AdminApp, IAdminAppBuilder {
		/// <summary>
		/// 网站启动时添加页面处理函数
		/// </summary>
		public void OnWebsiteStart() {

		}
	}
}
