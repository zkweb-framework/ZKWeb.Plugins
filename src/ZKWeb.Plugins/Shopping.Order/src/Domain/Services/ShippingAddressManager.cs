using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 收货地址管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ShippingAddressManager : DomainServiceBase<ShippingAddress, Guid> {
		/// <summary>
		/// 获取用户的收货地址列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public IList<ShippingAddress> GetShippingAddresses(Guid? userId) {
			if (userId == null) {
				return new List<ShippingAddress>();
			}
			return GetMany(query => {
				return query.Where(a => a.Owner.Id == userId)
					.OrderBy(a => a.DisplayOrder).ToList();
			});
		}
	}
}
