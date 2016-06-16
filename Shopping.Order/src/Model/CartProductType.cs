namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 购物车商品的类型
	/// 因为使用了固定的逻辑处理，这里不开放接口
	/// </summary>
	public enum CartProductType {
		/// <summary>
		/// 加入购物车
		/// </summary>
		Default = 0,
		/// <summary>
		/// 立即购买
		/// </summary>
		Buynow = 1
	}
}
