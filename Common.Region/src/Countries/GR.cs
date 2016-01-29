using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Region.src.Model;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 希腊
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GR : Country {
		public override string Name { get { return "GR"; } }
	}
}
