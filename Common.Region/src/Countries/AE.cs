using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 阿联酋
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AE : Country {
		public override string Name { get { return "AE"; } }
	}
}
