namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Structs {
	/// <summary>
	/// 商品关联的匹配数据，用于反序列化客户端传回的值
	/// 编辑商品时使用
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
	public class ProductMatchedDataForEdit {
		/// <summary>
		/// 匹配条件
		/// </summary>
		public ProductMatchedDataConditions Conditions { get; set; }
		/// <summary>
		/// 影响的数据
		/// </summary>
		public ProductMatchedDataAffects Affects { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMatchedDataForEdit() {
			Conditions = new ProductMatchedDataConditions();
			Affects = new ProductMatchedDataAffects();
		}
	}
}
