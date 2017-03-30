using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Controllers;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.UIComponents.EntityFormExtraHandlers {
	/// <summary>
	/// 给后台的订单详情页添加评价信息
	/// </summary>
	[ExportMany]
	public class AddRatingsToSellerOrderForm :
		IEntityFormExtraHandler<SellerOrder, Guid, SellerOrderCrudController.Form> {
		/// <summary>
		/// 评价表格
		/// </summary>
		[HtmlField("ProductRatings", Group = "ProductRating")]
		public HtmlString ProductRatings { get; set; }

		/// <summary>
		/// 创建表单时的处理
		/// </summary>
		public void OnCreated(SellerOrderCrudController.Form form) {
			form.AddFieldsFrom(this);
		}

		/// <summary>
		/// 绑定表单时的处理
		/// </summary>
		public void OnBind(SellerOrderCrudController.Form form, SellerOrder bindFrom) {
			var productRatingManager = Application.Ioc.Resolve<ProductRatingManager>();
			ProductRatings = productRatingManager.GetProductRatingsHtml(bindFrom);
		}

		/// <summary>
		/// 提交表单时的处理
		/// </summary>
		public void OnSubmit(SellerOrderCrudController.Form form, SellerOrder saveTo) {
		}

		/// <summary>
		/// 实体保存后的处理
		/// </summary>
		public void OnSubmitSaved(SellerOrderCrudController.Form form, SellerOrder saved) {
		}
	}
}
