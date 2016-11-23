using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 创建订单时的留言表单
	/// </summary>
	public class CreateOrderCommenForm : FieldsOnlyModelFormBuilder {
		/// <summary>
		/// 订单留言
		/// </summary>
		[TextAreaField("OrderComment", 5, "OrderComment")]
		public string OrderComment { get; set; }
	}
}
