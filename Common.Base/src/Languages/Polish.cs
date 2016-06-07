using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 波兰语
	/// </summary>
	[ExportMany]
	public class Polish : ILanguage {
		public string Name { get { return "pl-PL"; } }
	}
}
