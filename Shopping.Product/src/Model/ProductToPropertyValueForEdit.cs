using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品关联的属性值数据，用于反序列化客户端传回的值
	/// 编辑商品时使用
	/// 格式 { propertyId: 属性Id, propertyValueId: 属性值Id, name: 属性值名称 }
	/// 例
	/// 类目 [ 服装 ]
	/// 属性
	/// 年份 [2015] (文本框)
	/// 产地 [广东] (文本框)
	/// 适合季度 [春季] (下拉框)
	/// 规格
	/// 颜色 [✓蓝色] [✓红色] [灰色] (多选框)
	/// 尺码 [✓S] [M] [L] (多选框)
	/// [
	///		{ propertyId: 年份的Id, name: "2015" },
	///		{ propertyId: 产地的Id, name: "广东" },
	///		{ propertyId: 季度的Id, propertyValueId: 春季的Id, name: "春季" },
	///		{ propertyId: 颜色的Id, propertyValueId: 蓝色的Id, name: "蓝色" },
	///		{ propertyId: 颜色的Id, propertyValueId: 红色的Id, name: "红色" },
	///		{ propertyId: 尺码的Id, propertyValueId: S的Id, name: "S" }
	/// ]
	/// </summary>
	public class ProductToPropertyValueForEdit {
		/// <summary>
		/// 属性Id
		/// </summary>
		public long propertyId { get; set; }
		/// <summary>
		/// 属性值Id
		/// </summary>
		public long? propertyValueId { get; set; }
		/// <summary>
		/// 属性值名称
		/// </summary>
		public string name { get; set; }
	}
}
