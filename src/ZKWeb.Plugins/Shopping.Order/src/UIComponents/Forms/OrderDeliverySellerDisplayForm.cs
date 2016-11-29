using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 卖家查看的发货单详情页
	/// </summary>
	public class OrderDeliverySellerDisplayForm :
		OrderDeliveryBaseDisplayForm<OrderDeliverySellerDisplayForm> {
		/// <summary>
		/// 备注
		/// </summary>
		[TextAreaField("Remark", 5, Group = "Remark")]
		public string Remark { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(OrderDelivery bindFrom) {
			base.OnBind(bindFrom);
			Remark = bindFrom.Remark;
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(OrderDelivery saveTo) {
			saveTo.Remark = Remark;
			return this.SaveSuccessAndCloseModal();
		}
	}
}
