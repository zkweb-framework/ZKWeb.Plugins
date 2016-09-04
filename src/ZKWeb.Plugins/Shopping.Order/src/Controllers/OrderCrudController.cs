using System;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controller {
	/// <summary>
	/// 后台订单管理的控制器
	/// </summary>
	[ExportMany]
	public class OrderCrudController : AdminAppBuilder<Database.Order> {
		public override string Name { get { return "OrderManage"; } }
		public override string Url { get { return "/admin/orders"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		protected override IAjaxTableCallback<Database.Order> GetTableCallback() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }
	}
}
