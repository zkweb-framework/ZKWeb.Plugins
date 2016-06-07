using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 香港特别行政区
	/// </summary>
	[ExportMany, SingletonReuse]
	public class HK : Country {
		public override string Name { get { return "HK"; } }

		public HK() {
			RegionsCache = LazyCache.Create(() => {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var path = pathManager.GetResourceFullPath("texts", "regions_hk.json");
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<List<Model.Region>>(json);
			});
		}
	}
}
