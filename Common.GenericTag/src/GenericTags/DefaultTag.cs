using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.GenericTag.src.GenericTags {
	/// <summary>
	/// 默认标签
	/// </summary>
	[ExportMany]
	public class DefaultTag : GenericTagBuilder {
		public override string Name { get { return "DefaultTag"; } }
	}
}
