using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Model {
	/// <summary>
	/// 物流类型的接口
	/// </summary>
	public interface ILogisticsType {
		/// <summary>
		/// 物流类型
		/// </summary>
		string Type { get; }
	}
}
