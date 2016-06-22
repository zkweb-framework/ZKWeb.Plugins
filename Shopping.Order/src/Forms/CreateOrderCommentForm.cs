using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;

namespace ZKWeb.Plugins.Shopping.Order.src.Forms {
	/// <summary>
	/// 创建订单时的留言表单
	/// </summary>
	public class CreateOrderCommenForm : FieldsOnlyModelFormBuilder {
		/// <summary>
		/// 订单浏览
		/// </summary>
		[TextAreaField("OrderComment", 5, "OrderComment")]
		public string OrderComment { get; set; }
	}
}
