using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;

namespace ZKWeb.Plugins.Common.AdminSettings.src.Languages {
	/// <summary>
	/// 捷克语
	/// </summary>
	[ExportMany]
	public class Czech : ILanguage {
		public string Name { get { return "cs-CZ"; } }
	}
}
