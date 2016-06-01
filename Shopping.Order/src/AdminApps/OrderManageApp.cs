using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.AdminApps {
	/// <summary>
	/// 订单管理
	/// </summary>
	[ExportMany]
	public class OrderManageApp : AdminAppBuilder<Database.Order> {
		public override string Name { get { return "OrderManage"; } }
		public override string Url { get { return "/admin/orders"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		protected override IAjaxTableCallback<Database.Order> GetTableCallback() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }
	}
}
