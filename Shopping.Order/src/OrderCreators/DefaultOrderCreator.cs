using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.SerialGenerate.src.Generator;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderCreators {
	/// <summary>
	/// 默认的订单创建器
	/// 实现功能
	/// - 检查订单参数 (CheckOrderParameters)
	/// - 按卖家分别创建订单 (TODO)
	/// - 计算各个订单的价格 (TODO)
	/// - 添加关联的订单商品 (TODO)
	/// - 生成订单编号 (TODO)
	/// - 检查是否支持非会员下单
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
		/// 当前的创建订单的参数
		/// </summary>
		protected CreateOrderParameters Parameters { get; set; }
		/// <summary>
		/// 当前的数据库上下文
		/// </summary>
		protected DatabaseContext Context { get; set; }

		/// <summary>
		/// 检查订单参数
		/// - 商品是否存在
		/// - 库存是否足够
		/// - 是否有数量等于或小于0的商品
		/// - 如果有实体商品，必须选择物流
		/// - 如果有实体商品，必须填写收货地址
		/// </summary>
		public virtual void CheckOrderParameters() {
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var sellerToLogisticsId = Parameters.OrderParameters
				.GetOrDefault<IDictionary<long, long>>("SellerToLogistics") ?? new Dictionary<long, long>();
			var shippingAddress = Parameters.OrderParameters
				.GetOrDefault<IDictionary<string, object>>("ShippingAddress") ?? new Dictionary<string, object>();
			foreach (var productParameters in Parameters.OrderProductParametersList) {
				// 检查商品是否存在
				var product = productManager.GetProduct(productParameters.ProductId);
				if (product == null) {
					throw new BadRequestException(new T("Order contains product that not exist or deleted"));
				}
				// 检查库存是否足够
				var orderCount = productParameters.MatchParameters.GetOrDefault<long>("OrderCount");
				var data = product.MatchedDatas
					.Where(d => d.Stock != null)
					.WhereMatched(productParameters.MatchParameters).FirstOrDefault();
				if (data == null || data.Stock < orderCount) {
					throw new BadRequestException(string.Format(
						new T("Insufficient stock of product [{0}]"), new T(product.Name)));
				}
				// 是否有数量等于或小于0的商品
				if (orderCount <= 0) {
					throw new BadRequestException(new T("Order count must larger than 0"));
				}
				// 如果有实体商品，必须选择物流
				var typeTrait = product.GetTypeTrait();
				var sellerId = (product.Seller == null) ? 0 : product.Seller.Id;
				if (typeTrait.IsReal && sellerToLogisticsId.GetOrDefault(sellerId) <= 0) {
					throw new BadRequestException(
						new T("Order contains real products, please select a logistics"));
				}
				// 如果有实体商品，必须填写收货地址
				if (!typeTrait.IsReal) {
				} else if (string.IsNullOrEmpty(shippingAddress.GetOrDefault<string>("DetailedAddress"))) {
					throw new BadRequestException(new T("Please provide detailed address"));
				} else if (string.IsNullOrEmpty(shippingAddress.GetOrDefault<string>("ReceiverName"))) {
					throw new BadRequestException(new T("Please provide receiver name"));
				} else if (string.IsNullOrEmpty(shippingAddress.GetOrDefault<string>("ReceiverTel"))) {
					throw new BadRequestException(new T("Please provide receiver tel or mobile"));
				}
			}
		}

		/// <summary>
		/// 按卖家分别创建订单
		/// </summary>
		public virtual void CreateOrdersBySellers() {
			throw new NotImplementedException("not implemented");
			var userRepository = RepositoryResolver.Resolve<User>(Context);
			var order = new Database.Order() {
				Buyer = userRepository.GetById(Parameters.UserId),
			};
			order.Serial = SerialGenerator.GenerateFor(order);
		}

		/// <summary>
		/// 创建订单
		/// </summary>
		public virtual CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			var result = new CreateOrderResult();
			UnitOfWork.Write(context => {
				Parameters = parameters;
				Context = context;
				CheckOrderParameters();
				CreateOrdersBySellers();
			});
			return result;
		}
	}
}
