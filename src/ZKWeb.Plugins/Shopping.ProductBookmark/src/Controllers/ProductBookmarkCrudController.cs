using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Controllers {
	using ProductBookmark = Domain.Entities.ProductBookmark;

	/// <summary>
	/// 前台用户管理收藏的商品的控制器
	/// </summary>
	[ExportMany]
	public class ProductBookmarkCrudController : CrudUserPanelControllerBase<ProductBookmark, Guid> {
		public override string Group { get { return "ProductBookmark"; } }
		public override string GroupIconClass { get { return "fa fa-bookmark"; } }
		public override string Name { get { return "ProductBookmark"; } }
		public override string Url { get { return "/user/product_bookmarks"; } }
		public override string IconClass { get { return "fa fa-star"; } }
		public override string AddUrl { get { return null; } }
		public override bool AllowDeleteForever { get { return false; } }
		public override string EditTemplatePath { get { return "common.user_panel/generic_edit_standalone.html"; } }
		protected override IAjaxTableHandler<ProductBookmark, Guid> GetTableHandler() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }
	}
}
