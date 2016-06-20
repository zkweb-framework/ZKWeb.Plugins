using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Managers {
	/// <summary>
	/// 收货地址管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserShippingAddressManager {
		/// <summary>
		/// 获取用户的收货地址列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public IList<UserShippingAddress> GetShippingAddresses(long? userId) {
			if (userId == null) {
				return new List<UserShippingAddress>();
			}
			return UnitOfWork.ReadData<UserShippingAddress, List<UserShippingAddress>>(r => {
				return r.GetMany(a => a.User.Id == userId && !a.Deleted)
					.OrderBy(a => a.DisplayOrder).ToList();
			});
		}
	}
}
