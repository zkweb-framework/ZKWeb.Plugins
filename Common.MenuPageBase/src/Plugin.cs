using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.MenuPageBase.src {
	/// <summary>
	/// 网站启动时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册通用菜单页
			Application.Ioc.ResolveMany<GenericPageForMenuPage>().ForEach(g => g.OnWebsiteStart());
		}
	}
}
