using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 繁体中文
	/// </summary>
	[ExportMany]
	public class TraditionalChinese : ILanguage {
		public string Name { get { return "zh-TW"; } }
	}
}
