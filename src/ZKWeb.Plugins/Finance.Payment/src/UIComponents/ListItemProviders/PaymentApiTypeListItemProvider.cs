using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.ListItemProviders {
	/// <summary>
	/// 支付接口类型列表
	/// </summary>
	public class PaymentApiTypeListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (var handler in Application.Ioc.ResolveMany<IPaymentApiHandler>()) {
				yield return new ListItem(new T(handler.Type), handler.Type);
			}
		}
	}
}
