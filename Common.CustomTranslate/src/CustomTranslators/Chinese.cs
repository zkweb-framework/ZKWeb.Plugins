using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 中文
	/// </summary>
	[ExportMany]
	public class Chinese : CustomTranslator {
		public override string Name { get { return "zh-CN"; } }
	}
}
