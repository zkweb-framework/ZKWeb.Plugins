using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Scaffolding;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.UserPanelPages {
	/// <summary>
	/// 订单列表
	/// </summary>
	[ExportMany]
	public class OrderManage : UserPanelCrudPageBuilder<Database.Order> {
		public override string Group { get { return "OrderManage"; } }
		public override string GroupIconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string Name { get { return "OrderList"; } }
		public override string Url { get { return "/user/orders"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string AddUrl { get { return null; } }
		public override string EditUrl { get { return null; } }
		public override string BatchUrl { get { return null; } }
		protected override IAjaxTableCallback<Database.Order> GetTableCallback() { throw new NotImplementedException(); }
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
			groups.Insert(1, group);
			var itemIndex = group.Items.Count - 1;
			var item = group.Items[itemIndex];
			group.Items.RemoveAt(itemIndex);
			group.Items.Insert(0, item);
		}
	}
}
