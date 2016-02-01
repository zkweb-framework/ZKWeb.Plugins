using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 西班牙语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Spanish : CustomTranslator {
		public override string Name { get { return "es-ES"; } }
	}
}
