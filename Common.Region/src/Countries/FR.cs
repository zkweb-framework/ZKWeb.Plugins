using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 法国
	/// </summary>
	[ExportMany, SingletonReuse]
	public class FR : Country {
		public override string Name { get { return "FR"; } }

		public FR() {
			RegionsCache = LazyCache.Create(() => {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var path = pathManager.GetResourceFullPath("texts", "regions_fr.json");
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<List<Model.Region>>(json);
			});
		}
	}
}
