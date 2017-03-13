using System;
using System.Linq;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Region.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Region.src.Domain.Services;
using ZKWeb.Plugins.Common.Region.src.UIComponents.ListItemProviders;
using System.Threading;

namespace ZKWeb.Plugins.Common.Region.src.Controllers {
	/// <summary>
	/// 地区使用的Api控制器
	/// </summary>
	[ExportMany]
	public class RegionApiController : ControllerBase {
		/// <summary>
		/// 获取国家信息
		/// 返回默认国家和国家列表
		/// </summary>
		/// <returns></returns>
		[Action("api/region/country_info", HttpMethods.POST)]
		public IActionResult CountryInfo() {
			Thread.Sleep(3000);
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<RegionSettings>();
			var defaultCountry = settings.DefaultCountry;
			var countries = new CountryListItemProvider().GetItems().ToList();
			return new JsonResult(new { defaultCountry, countries });
		}

		/// <summary>
		/// 获取指定国家的地区树
		/// </summary>
		/// <returns></returns>
		[Action("api/region/region_tree_from_country", HttpMethods.POST)]
		public IActionResult RegionTreeFromCountry() {
			Thread.Sleep(3000);
			var countryName = HttpManager.CurrentContext.Request.Get<string>("country");
			var regionManager = Application.Ioc.Resolve<RegionManager>();
			var country = regionManager.GetCountry(countryName);
			var tree = country == null ? null : country.GetRegionsTree();
			return new JsonResult(new { tree });
		}
	}
}
