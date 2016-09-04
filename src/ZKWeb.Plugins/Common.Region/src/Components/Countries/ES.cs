using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Components.Countries {
	/// <summary>
	/// 西班牙
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ES : Country {
		public override string Name { get { return "ES"; } }

		public ES() {
			RegionsCache = LazyCache.Create(() => {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var path = pathManager.GetResourceFullPath("texts", "regions_es.json");
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<List<Regions.Region>>(json);
			});
		}
	}
}
