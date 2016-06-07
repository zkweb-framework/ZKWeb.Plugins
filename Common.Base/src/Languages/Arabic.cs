using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 阿拉伯语
	/// </summary>
	[ExportMany]
	public class Arabic : ILanguage {
		public string Name { get { return "ar-DZ"; } }
	}
}
