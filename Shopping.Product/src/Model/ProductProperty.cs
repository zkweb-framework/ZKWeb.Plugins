using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品属性
	/// 不同类目下的商品属性即时Id一样内容也有可能不一样，获取时注意判断ParentCategoryIds
	/// 这个对象中的值生成后不应该修改
	/// </summary>
	public class ProductProperty : ILiquidizable {
		/// <summary>
		/// 属性Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 所属的商品类目Id列表
		/// </summary>
		public IList<long> ParentCategoryIds { get; set; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 是否销售属性
		/// 销售属性时买家在购买时必须选择该属性的值
		/// </summary>
		public bool IsSaleProperty { get; set; }
		/// <summary>
		/// 是否颜色属性
		/// </summary>
		public bool IsColorProperty { get; set; }
		/// <summary>
		/// 输入控件类型
		/// </summary>
		public ProductPropertyControlType ControlType { get; set; }
		/// <summary>
		/// 属性值的Id列表
		/// </summary>
		public IList<long> PropertyValueIds { get; set; }

		/// <summary>
		/// 支持描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new {
				Id, Name, ParentCategoryIds,
				IsSaleProperty, IsColorProperty, ControlType, PropertyValueIds
			};
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		public override string ToString() {
			return Name;
		}
	}
}
