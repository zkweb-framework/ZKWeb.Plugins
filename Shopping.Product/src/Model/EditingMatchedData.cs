using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 用于编辑的商品匹配数据
	/// 这个值用于反序列化编辑商品传回的数据
	/// 例
	/// 条件								价格		货币		重量		库存		备注		调整
	/// 颜色: 蓝, 大小 M, 订购数量 >= 1	120		默认		继承		100		新版		上下
	/// 默认								100  	默认		500		101
	/// [
	/// {
	///		Conditions: {
	///			Properties: [
	///				{ PropertyId: 颜色的Id, PropertyValueId: 蓝色的Id },
	///				{ PropertyId: 大小的Id, PropertyValueId: M的Id }
	///			],
	///			OrderCountGE: 1
	///		},
	///		Affects: {
	///			Price: 120,
	///			PriceCurrency: 0,
	///			Weight: null,
	///			Stock: 100,
	///			Remark: "新版"
	///		}
	/// },
	/// {
	///		Conditions: {},
	///		Affects: {
	///			Price: 100,
	///			PriceCurrency: 0,
	///			Weight: 500,
	///			Stock: 101,
	///			Remark: ""
	///		}
	/// }
	/// ]
	/// </summary>
	public class EditingMatchedData {
		/// <summary>
		/// 匹配条件
		/// </summary>
		public Dictionary<string, object> Conditions { get; set; }
		/// <summary>
		/// 影响的数据
		/// </summary>
		public Dictionary<string, object> Affects { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public EditingMatchedData() {
			Conditions = new Dictionary<string, object>();
			Affects = new Dictionary<string, object>();
		}
	}
}
