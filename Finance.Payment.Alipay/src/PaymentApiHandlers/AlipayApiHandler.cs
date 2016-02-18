using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.PaymentApiHandlers {
	/// <summary>
	/// 支付宝接口的处理器
	/// </summary>
	[ExportMany]
	public class TestApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "Alipay"; } }
		/// <summary>
		/// 后台编辑表单使用的接口数据
		/// </summary>
		protected ApiData ApiDataEditing = new ApiData();

		/// <summary>
		/// 后台编辑表单创建后的处理
		/// </summary>
		public void OnFormCreated(PaymentApiEditForm form) {
			form.AddFieldsFrom(ApiDataEditing);
		}

		/// <summary>
		/// 后台编辑表单绑定时的处理
		/// </summary>
		public void OnFormBind(PaymentApiEditForm form, DatabaseContext context, PaymentApi bindFrom) {
			var apiData = bindFrom.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			apiData.CopyMembersTo(ApiDataEditing);
		}

		/// <summary>
		/// 后台编辑表单保存时的处理
		/// </summary>
		public void OnFormSubmit(PaymentApiEditForm form, DatabaseContext context, PaymentApi saveTo) {
			saveTo.ExtraData["ApiData"] = ApiDataEditing;
		}

		/// <summary>
		/// 获取支付Html
		/// </summary>
		public void GetPaymentHtml(PaymentTransaction transaction, ref HtmlString html) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 调用发货接口
		/// </summary>
		public void SendGoods(PaymentTransaction transaction, string logisticsName, string invoiceNo) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 接口数据
		/// </summary>
		public class ApiData {
			/// <summary>
			/// 商户Id
			/// </summary>
			[Required]
			[TextBoxField("PartnerId", "PartnerId")]
			public string PartnerId { get; set; }
			/// <summary>
			/// 商户邮箱
			/// </summary>
			[Required]
			[TextBoxField("PartnerEmail", "PartnerEmail")]
			public string PartnerEmail { get; set; }
			/// <summary>
			/// 商户密钥
			/// </summary>
			[Required]
			[TextBoxField("PartnerKey", "PartnerKey")]
			public string PartnerKey { get; set; }
			/// <summary>
			/// 服务类型
			/// </summary>
			[Required]
			[RadioButtonsField("ServiceType", typeof(ListItemFromEnum<AlipayServiceTypes>))]
			public int ServiceType { get; set; }
			/// <summary>
			/// 返回域名
			/// </summary>
			[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
			public string ReturnDomain { get; set; }
		}
	}
}
