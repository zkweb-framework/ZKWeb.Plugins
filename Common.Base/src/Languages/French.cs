using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 法语
	/// </summary>
	[ExportMany]
	public class French : ILanguage {
		public string Name { get { return "fr-FR"; } }
	}
}
