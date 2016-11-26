using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerate;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 订单发货管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class OrderDeliveryManager : DomainServiceBase<OrderDelivery, Guid> {
		/// <summary>
		/// 对订单进行发货
		/// 当所有商品已发货时，设置订单为已发货
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <param name="operatorId">发货人Id</param>
		/// <param name="logisticsId">物流Id</param>
		/// <param name="invoiceNo">快递单编号</param>
		/// <param name="remark">备注</param>
		/// <param name="deliveryCounts">订单商品Id到发货数量的索引</param>
		public virtual void DeliveryGoods(
			Guid orderId, Guid? operatorId,
			Guid logisticsId, string invoiceNo,
			string remark, IDictionary<Guid, long> deliveryCounts) {
			var unitOfWork = Application.Ioc.Resolve<IUnitOfWork>();
			using (unitOfWork.Scope()) {
				// 开始事务
				unitOfWork.Context.BeginTransaction();
				// 获取订单
				var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
				var order = orderManager.Get(orderId);
				if (order == null) {
					throw new BadRequestException(new T("Order not exist"));
				}
				// 获取物流
				var containsRealProduct = order.ContainsRealProduct();
				var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
				var logistics = containsRealProduct ? logisticsManager.Get(logisticsId) : null;
				if (!containsRealProduct) {
					// 虚拟商品不需要物流和快递单编号
				} else if (logistics == null) {
					throw new BadRequestException(new T("Selected logistics does not exist"));
				} else if (string.IsNullOrEmpty(invoiceNo)) {
					throw new BadRequestException(new T("Please provide logistics serial (invoice no)"));
				}
				// 检查发货数量
				if (deliveryCounts.Sum(c => c.Value) <= 0) {
					throw new BadRequestException(new T("No products to be delivery"));
				}
				var unshippedMapping = GetUnshippedProductMapping(order);
				foreach (var count in deliveryCounts) {
					if (!unshippedMapping.ContainsKey(count.Key)) {
						throw new BadRequestException(new T("Can't delivery product that not exists in order"));
					} else if (count.Value < 0) {
						throw new BadRequestException(new T("Invalid delivery count"));
					} else if (count.Value > unshippedMapping[count.Key]) {
						throw new BadRequestException(new T("Delivery count can't be larger than unshipped count"));
					}
				}
				// 添加发货单
				var userManager = Application.Ioc.Resolve<UserManager>();
				var delivery = new OrderDelivery() {
					Order = order,
					Logistics = logistics,
					LogisticsSerial = invoiceNo,
					Operator = operatorId.HasValue ? userManager.Get(operatorId.Value) : null,
					Remark = remark
				};
				delivery.Serial = SerialGenerator.GenerateFor(delivery);
				Save(ref delivery);
				foreach (var count in deliveryCounts.Where(p => p.Value > 0)) {
					var orderProduct = order.OrderProducts.First(p => p.Id == count.Key);
					var orderDeliveryToOrderProduct = new OrderDeliveryToOrderProduct() {
						OrderDelivery = delivery,
						OrderProduct = orderProduct,
						Count = count.Value
					};
					orderProduct.Deliveries.Add(orderDeliveryToOrderProduct);
				}
				var orderRepository = Application.Ioc.Resolve<IRepository<SellerOrder, Guid>>();
				orderRepository.Save(ref order, null);
				// 当所有商品已发货时，设置订单为已发货
				if (deliveryCounts.Sum(c => c.Value) == unshippedMapping.Sum(c => c.Value)) {
					var canProcessAllGoodsShipped = order.Check(c => c.CanProcessAllGoodsShipped);
					if (!canProcessAllGoodsShipped.First) {
						throw new BadRequestException(canProcessAllGoodsShipped.Second);
					}
					orderManager.ProcessAllGoodsShipped(orderId);
				}
				// 结束事务
				unitOfWork.Context.FinishTransaction();
			}
		}

		/// <summary>
		/// 获取未发货的订单商品数量索引
		/// 返回 { 订单商品Id: 未发数量 }
		/// </summary>
		/// <param name="order">订单</param>
		/// <returns></returns>
		public virtual IDictionary<Guid, long> GetUnshippedProductMapping(SellerOrder order) {
			return order.OrderProducts.ToDictionary(
				p => p.Id, p => p.Count - p.Deliveries.Sum(d => d.Count));
		}

		/// <summary>
		/// 获取已发货的订单商品数量索引
		/// 返回 { 订单商品Id: 已发数量 }
		/// </summary>
		/// <param name="order">订单</param>
		/// <returns></returns>
		public virtual IDictionary<Guid, long> GetShippedProductMapping(SellerOrder order) {
			return order.OrderProducts.ToDictionary(
				p => p.Id, p => p.Deliveries.Sum(d => d.Count));
		}
	}
}
