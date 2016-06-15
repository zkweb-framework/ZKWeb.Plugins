using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Region.src.Model {
	/// <summary>
	/// 国家和地区
	/// 地区联动下拉框使用
	/// </summary>
	public class CountryAndRegion {
		/// <summary>
		/// 国家
		/// </summary>
		public string Country { get; set; }
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
		/// <param name="country">国家Id</param>
		/// <param name="regionId">地区Id</param>
		public CountryAndRegion(string country, long? regionId) {
			Country = country;
			RegionId = regionId;
		}
	}
}
