using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 俄罗斯
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RU : Country {
		public override string Name { get { return "RU"; } }
	}
}
