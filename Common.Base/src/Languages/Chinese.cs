using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Languages {
	/// <summary>
	/// 中文
	/// </summary>
	[ExportMany]
	public class Chinese : ILanguage {
		public string Name { get { return "zh-CN"; } }
	}
}
