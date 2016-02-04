using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Finance.Payment.src.Database;

namespace ZKWeb.Plugins.Finance.Payment.src.Forms {
	/// <summary>
	/// 编辑支付接口使用的表单
	/// </summary>
	public class PaymentApiEditForm : DataEditFormBuilder<PaymentApi, PaymentApiEditForm> {
		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		protected override void OnBind(DatabaseContext context, PaymentApi bindFrom) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 保存表单到数据
		/// </summary>
		protected override object OnSubmit(DatabaseContext context, PaymentApi saveTo) {
			throw new NotImplementedException();
		}
	}
}
