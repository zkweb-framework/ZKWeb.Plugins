using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 繁体中文
	/// </summary>
	[ExportMany, SingletonReuse]
	public class TraditionalChinese : CustomTranslator {
		public override string Name { get { return "zh-TW"; } }
	}
}
