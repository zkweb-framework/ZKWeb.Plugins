using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Components.Countries {
	/// <summary>
	/// 俄罗斯
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RU : Country {
		public override string Name { get { return "RU"; } }
	}
}
