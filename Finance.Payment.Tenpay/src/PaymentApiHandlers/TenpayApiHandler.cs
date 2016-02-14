using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.Tenpay.src.PaymentApiHandlers {
	/// <summary>
	/// 财付通接口的处理器
	/// </summary>
	[ExportMany]
	public class TenpayApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "Tenpay"; } }
		/// <summary>
		/// 商户Id
		/// </summary>
		[Required]
		[TextBoxField("PartnerId", "PartnerId")]
		public string PartnerId { get; set; }
		/// <summary>
		/// 商户密钥
		/// </summary>
		[Required]
		[TextBoxField("PartnerKey", "PartnerKey")]
		public string PartnerKey { get; set; }
		/// <summary>
		/// 返回域名
		/// </summary>
		[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
		public string ReturnDomain { get; set; }
		
		/// <summary>
		/// 后台编辑表单创建后的处理
		/// </summary>
		public void OnFormCreated(PaymentApiEditForm form) {
			form.AddFieldsFrom(this);
		}

		/// <summary>
		/// 后台编辑表单绑定时的处理
		/// </summary>
		public void OnFormBind(PaymentApiEditForm form, DatabaseContext context, PaymentApi bindFrom) {
			PartnerId = bindFrom.ExtraData.GetOrDefault<string>("PartnerId");
			PartnerKey = bindFrom.ExtraData.GetOrDefault<string>("PartnerKey");
			ReturnDomain = bindFrom.ExtraData.GetOrDefault<string>("ReturnDomain");
		}

		/// <summary>
		/// 后台编辑表单保存时的处理
		/// </summary>
		public void OnFormSubmit(PaymentApiEditForm form, DatabaseContext context, PaymentApi saveTo) {
			saveTo.ExtraData["PartnerId"] = PartnerId;
			saveTo.ExtraData["PartnerKey"] = PartnerKey;
			saveTo.ExtraData["ReturnDomain"] = ReturnDomain;
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
	}
}
