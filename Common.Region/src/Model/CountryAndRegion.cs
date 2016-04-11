using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Region.src.Model {
	/// <summary>
	/// 国家和地区
	/// 地区联动下拉框使用
	/// </summary>
	public class CountryAndRegion {
		/// <summary>
		/// 国家Id
		/// </summary>
		public long CountryId { get; set; }
		/// <summary>
		/// 地区Id
		/// </summary>
		public long? RegionId { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CountryAndRegion() {
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="countryId">国家Id</param>
		/// <param name="regionId">地区Id</param>
		public CountryAndRegion(long countryId, long? regionId) {
			CountryId = countryId;
			RegionId = regionId;
		}
	}
}
