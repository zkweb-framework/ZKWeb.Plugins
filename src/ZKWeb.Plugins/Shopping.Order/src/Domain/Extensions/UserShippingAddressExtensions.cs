using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.Extensions;
using ZKWeb.Plugins.Common.Region.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Database;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 收货地址的扩展函数
	/// </summary>
	public static class UserShippingAddressExtensions {
		/// <summary>
		/// 生成完整的收货地址字符串
		/// </summary>
		/// <param name="address">收货地址</param>
		/// <returns></returns>
		public static string GenerateSummary(this UserShippingAddress address) {
			// 获取国家和地区名称
			// 如果设置了不显示国家下拉框，则国家名称等于空
			var regionManager = Application.Ioc.Resolve<RegionManager>();
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var country = regionManager.GetCountry(address.Country) ?? regionManager.GetDefaultCountry();
			var region = (address.RegionId == null ? null :
				country.GetRegionsTreeNode(address.RegionId.Value));
			var regionSettings = configManager.GetData<RegionSettings>();
			var countryName = regionSettings.DisplayCountryDropdown ? new T(country.Name) : "";
			var regionName = region == null ? null : region.GetFullname();
			// 生成完整的收货地址字符串
			return string.Format("{0} {1}{2}{3} {4}",
				address.ReceiverName, countryName, regionName,
				address.DetailedAddress, address.ReceiverTel);
		}
	}
}
