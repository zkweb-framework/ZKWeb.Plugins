using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 当前登录用户的收货地址的选项列表
	/// </summary>
	[ExportMany(ContractKey = "ShippingAddressListItemProvider")]
	public class ShippingAddressListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = user?.Id;
			foreach (var address in orderManager.GetAvailableShippingAddress(userId)) {
				yield return new ListItem(address.Summary, JsonConvert.SerializeObject(address.ToLiquid()));
			}
			yield return new ListItem(new T("Use new address"), "{}");
		}
	}
}
