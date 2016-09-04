using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Components.Countries {
	/// <summary>
	/// 阿联酋
	/// </summary>
	[ExportMany, SingletonReuse]
	public class AE : Country {
		public override string Name { get { return "AE"; } }
	}
}
