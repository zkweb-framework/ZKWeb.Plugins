using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 阿拉伯语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Arabic : CustomTranslator {
		public override string Name { get { return "ar-DZ"; } }
	}
}
