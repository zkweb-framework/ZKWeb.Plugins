using DryIoc;
using DryIocAttributes;
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

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 美国
	/// </summary>
	[ExportMany, SingletonReuse]
	public class US : Country {
		public override string Name { get { return "US"; } }

		public US() {
			RegionsCache = LazyCache.Create(() => {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var path = pathManager.GetResourceFullPath("texts", "regions_us.json");
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<List<Model.Region>>(json);
			});
		}
	}
}
