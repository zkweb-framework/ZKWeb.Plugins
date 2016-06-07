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
	/// 韩国
	/// </summary>
	[ExportMany, SingletonReuse]
	public class KR : Country {
		public override string Name { get { return "KR"; } }

		public KR() {
			RegionsCache = LazyCache.Create(() => {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				var path = pathManager.GetResourceFullPath("texts", "regions_kr.json");
				var json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<List<Model.Region>>(json);
			});
		}
	}
}
