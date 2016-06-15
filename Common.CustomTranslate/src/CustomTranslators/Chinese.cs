using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 中文
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Chinese : CustomTranslator {
		public override string Name { get { return "zh-CN"; } }
	}
}
