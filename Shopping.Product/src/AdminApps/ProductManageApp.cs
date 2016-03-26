using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品管理
	/// </summary>
	[ExportMany]
	public class ProductManageApp : AdminAppBuilder<Database.Product, ProductManageApp> {
		public override string Name { get { return "ProductManage"; } }
		public override string Url { get { return "/admin/products"; } }
		public override string TileClass { get { return "tile bg-green"; } }
		public override string IconClass { get { return "fa fa-diamond"; } }
		protected override IAjaxTableCallback<Database.Product> GetTableCallback() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }
	}
}
