using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;

namespace ZKWeb.Plugins.Shopping.Order.src.Forms {
	/// <summary>
	/// 用于创建订单的表单
	/// </summary>
	[Form("CreateOrderForm", "/order/create_order", SubmitButtonText = "SubmitOrder")]
	public class CreateOrderForm : ModelFormBuilder {
		/// <summary>
		/// 创建订单的参数，格式是Json
		/// </summary>
		[Required]
		[HiddenField("CreateOrderParameters")]
		public string CreateOrderParameters { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			// 反序列化参数
			// 调用订单管理器创建订单
			throw new NotImplementedException();
		}
	}
}
