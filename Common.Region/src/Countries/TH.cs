using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 泰国
	/// </summary>
	[ExportMany, SingletonReuse]
	public class TH : Country {
		public override string Name { get { return "TH"; } }
	}
}
