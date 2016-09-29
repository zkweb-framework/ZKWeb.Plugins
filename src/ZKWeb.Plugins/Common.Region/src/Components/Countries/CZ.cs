using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWeb.Storage;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Components.Countries {
	/// <summary>
	/// 捷克
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CZ : Country {
		public override string Name { get { return "CZ"; } }

		public CZ() {
			RegionsCache = LazyCache.Create(() => {
				var fileStorage = Application.Ioc.Resolve<IFileStorage>();
				var json = fileStorage.GetResourceFile("texts", "regions_cz.json").ReadAllText();
				return JsonConvert.DeserializeObject<List<Regions.Region>>(json);
			});
		}
	}
}
