using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.UserPanelPages {
	/// <summary>
	/// 前台的订单管理的控制器
	/// </summary>
	[ExportMany]
	public class BuyerOrderCrudController : CrudUserPanelControllerBase<BuyerOrder, Guid> {
		public override string Group { get { return "OrderManage"; } }
		public override string GroupIconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string Name { get { return "OrderList"; } }
		public override string Url { get { return "/user/orders"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string AddUrl { get { return null; } }
		public override string EditUrl { get { return null; } }
		public override string BatchUrl { get { return null; } }
		protected override IAjaxTableHandler<BuyerOrder, Guid> GetTableHandler() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }

		/// <summary>
		/// 调整订单管理和订单列表的位置
		/// </summary>
		/// <param name="groups">菜单项列表的分组</param>
		public override void Setup(IList<MenuItemGroup> groups) {
			base.Setup(groups);
			var groupIndex = groups.FindIndex(g => g.Name == Group);
			var group = groups[groupIndex];
			groups.RemoveAt(groupIndex);
			groups.Insert(1, group); // 订单管理排第二（仅次于首页）
			var itemIndex = group.Items.Count - 1;
			var item = group.Items[itemIndex];
			group.Items.RemoveAt(itemIndex);
			group.Items.Insert(0, item); // 订单列表在订单管理中排第一
		}
	}
}
