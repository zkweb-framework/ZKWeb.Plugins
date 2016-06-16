namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品关联的属性值数据，用于反序列化客户端传回的值
	/// 商品匹配参数中使用
	/// </summary>
	public class ProductToPropertyValueForMatch {
		/// <summary>
		/// 属性Id
		/// </summary>
		public long PropertyId { get; set; }
		/// <summary>
		/// 属性值Id
		/// </summary>
		public long? PropertyValueId { get; set; }
		/// <summary>
		/// 属性值名称
		/// </summary>
		public string PropertyValueName { get; set; }
	}
}
