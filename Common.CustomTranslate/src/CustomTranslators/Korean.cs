using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.CustomTranslators {
	/// <summary>
	/// 韩语
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Korean : CustomTranslator {
		public override string Name { get { return "ko-KR"; } }
	}
}
