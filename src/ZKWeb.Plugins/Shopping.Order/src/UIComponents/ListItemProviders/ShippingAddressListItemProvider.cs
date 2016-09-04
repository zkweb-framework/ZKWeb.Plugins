using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Managers;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 当前登录用户的收货地址的选项列表
	/// </summary>
	public class ShippingAddressListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = (user == null) ? null : (long?)user.Id;
			foreach (var address in orderManager.GetAvailableShippingAddress(userId)) {
				yield return new ListItem(address.Summary, JsonConvert.SerializeObject(address.ToLiquid()));
			}
			yield return new ListItem(new T("Use new address"), "{}");
		}
	}
}
