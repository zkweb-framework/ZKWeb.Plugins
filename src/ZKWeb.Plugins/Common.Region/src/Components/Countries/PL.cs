using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWeb.Storage;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Components.Countries {
	/// <summary>
	/// 波兰
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PL : Country {
		public override string Name { get { return "PL"; } }

		public PL() {
			RegionsCache = LazyCache.Create(() => {
				var fileStorage = Application.Ioc.Resolve<IFileStorage>();
				var json = fileStorage.GetResourceFile("texts", "regions_pl.json").ReadAllText();
				return JsonConvert.DeserializeObject<List<Regions.Region>>(json);
			});
		}
	}
}
