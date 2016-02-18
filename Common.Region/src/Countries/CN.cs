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

namespace ZKWeb.Plugins.Common.Region.src.Countries {
	/// <summary>
	/// 中国
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CN : Country {
		public override string Name { get { return "CN"; } }

		public CN() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetResourceFullPath("texts", "regions_cn.json");
			if (path != null) {
				var json = File.ReadAllText(path);
				var regions = JsonConvert.DeserializeObject<IEnumerable<Model.Region>>(json);
				Regions.AddRange(regions);
			}
		}
	}
}
