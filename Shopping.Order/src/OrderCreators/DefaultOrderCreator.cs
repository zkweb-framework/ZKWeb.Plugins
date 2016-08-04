using System;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderCreators {
	/// <summary>
	/// 默认的订单创建器
	/// 实现功能
	/// - 按卖家分别创建订单 (TODO)
	/// - 计算各个订单的价格 (TODO)
	/// - 检查订单参数 (TODO)
	///   - 商品是否存在
	///   - 库存是否足够
	///   - 价格是否大于0
	///   - 是否有数量等于或小于0的商品
	///   - 如果有实体商品，必须选择物流
	///   - 如果有实体商品，必须填写收货地址
	/// - 生成订单编号 (TODO)
	/// - 添加关联的订单商品 (TODO)
	/// - 删除相应的购物车商品 (TODO)
	/// - 添加关联的订单留言 (TODO)
	/// - 保存收货地址的修改 (TODO)
	/// - 如果设置了下单时扣减库存，减少对应商品的库存 (TODO)
	/// - 创建订单交易 (TODO)
	/// - 有一个以上的订单时创建合并订单交易 (TODO)
	/// </summary>
	[ExportMany]
	public class DefaultOrderCreator : IOrderCreator {
		/// <summary>
		/// 创建订单
		/// </summary>
		public CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			throw new NotImplementedException("Order creator not implemented");
		}
	}
}
