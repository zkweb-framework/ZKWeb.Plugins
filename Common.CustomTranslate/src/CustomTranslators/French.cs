using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 法语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class French : CustomTranslator {
		public override string Name { get { return "fr-FR"; } }
	}
}
