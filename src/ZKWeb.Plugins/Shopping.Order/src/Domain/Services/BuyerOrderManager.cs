using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 买家订单管理器
	/// 包含买家使用的订单操作
	/// </summary>
	[ExportMany, SingletonReuse]
	public class BuyerOrderManager : DomainServiceBase<BuyerOrder, Guid> {
		/// <summary>
		/// 根据订单编号获取买家订单Id
		/// 失败时返回null
		/// </summary>
		/// <param name="serial">订单编号</param>
		/// <returns></returns>
		public virtual Guid? GetBuyerOrderIdFromSerial(string serial) {
			using (UnitOfWork.Scope()) {
				var orderId = Repository.Query()
					.Where(o => o.SellerOrder.Serial == serial)
					.Select(o => o.Id)
					.FirstOrDefault();
				return orderId == Guid.Empty ? null : (Guid?)orderId;
			}
		}

		/// <summary>
		/// 根据订单编号列表获取买家订单Id列表
		/// 返回的数量不一定一致
		/// </summary>
		/// <param name="serials">订单编号</param>
		/// <returns></returns>
		public virtual IList<Guid> GetBuyerOrderIdsFromSerials(IList<string> serials) {
			using (UnitOfWork.Scope()) {
				var orderIds = Repository.Query()
					.Where(o => serials.Contains(o.SellerOrder.Serial))
					.Select(o => o.Id)
					.ToList();
				return orderIds;
			}
		}

		public virtual Guid GetSellerOrderIdFromBuyerOrderId(Guid orderId) {
			using (UnitOfWork.Scope()) {
				var sellerOrderId = Repository.Query()
					.Where(o => o.Id == orderId)
					.Select(o => o.SellerOrder.Id)
					.FirstOrDefault();
				return sellerOrderId;
			}
		}

		/// <summary>
		/// 取消订单
		/// </summary>
		/// <param name="orderId">买家订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废理由，必填</param>
		/// <returns></returns>
		public virtual bool CancelOrder(Guid orderId, Guid? operatorId, string reason) {
			using (UnitOfWork.Scope()) {
				var sellerOrderId = GetSellerOrderIdFromBuyerOrderId(orderId);
				var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
				return sellerOrderManager.CancelOrder(sellerOrderId, operatorId, reason);
			}
		}

		/// <summary>
		/// 确认收货
		/// </summary>
		/// <param name="orderId">买家订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <returns></returns>
		public virtual bool ConfirmOrder(Guid orderId, Guid? operatorId) {
			using (UnitOfWork.Scope()) {
				var sellerOrderId = GetSellerOrderIdFromBuyerOrderId(orderId);
				var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
				return sellerOrderManager.ConfirmOrder(sellerOrderId, operatorId, true);
			}
		}

		/// <summary>
		/// 合并支付
		/// 返回合并后的支付交易Id
		/// </summary>
		/// <param name="orderIds">买家Id列表</param>
		/// <returns></returns>
		public virtual Guid MergePayment(IList<Guid> orderIds) {
			Guid result;
			using (UnitOfWork.Scope()) {
				// 开始事务
				UnitOfWork.Context.BeginTransaction();
				// 获取订单并检查是否都可以支付
				var orders = GetMany(o => orderIds.Contains(o.Id));
				if (orders.Any(o => !o.SellerOrder.Check(c => c.CanPay).First)) {
					throw new BadRequestException(new T("Some order is not payable"));
				}
				// 创建合并交易
				var sellerOrderManager = Application.Ioc.Resolve<SellerOrderManager>();
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				var childTransactions = orders.SelectMany(o =>
					sellerOrderManager.GetReleatedTransactions(o.SellerOrder.Id)).ToList();
				var transaction = transactionManager.CreateMergedTransaction(
					MergedOrderTransactionHandler.ConstType, childTransactions);
				result = transaction.Id;
				// 结束事务
				UnitOfWork.Context.FinishTransaction();
			}
			return result;
		}
	}
}
