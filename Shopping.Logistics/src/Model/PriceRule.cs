using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Model {
	/// <summary>
	/// 物流费用计算规则
	/// </summary>
	public class PriceRule {
		/// <summary>
		/// 首重单位，默认是1000G
		/// </summary>
		public long FirstHeavyUnit { get; set; }
		/// <summary>
		/// 首重费用
		/// </summary>
		public decimal FirstHeavyCost { get; set; }
		/// <summary>
		/// 续重单位，默认是1000G
		/// </summary>
		public long ContinuedHeavyUnit { get; set; }
		/// <summary>
		/// 续重费用
		/// </summary>
		public decimal ContinuedHeavyCost { get; set; }
		/// <summary>
		/// 货币单位
		/// </summary>
		public string Currency { get; set; }
		/// <summary>
		/// 国家，匹配所有时等于null
		/// </summary>
		public string Country { get; set; }
		/// <summary>
		/// 地区Id，匹配所有时等于null
		/// </summary>
		public long? RegionId { get; set; }
		/// <summary>
		/// 是否不允许在该地区使用
		/// </summary>
		public bool Disabled { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PriceRule() {
			FirstHeavyUnit = 1000;
			ContinuedHeavyUnit = 1000;
		}
	}
}
