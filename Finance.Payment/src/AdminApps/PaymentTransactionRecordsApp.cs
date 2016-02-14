using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;

namespace ZKWeb.Plugins.Finance.Payment.src.AdminApps {
	/// <summary>
	/// 支付交易记录
	/// </summary>
	[ExportMany]
	public class PaymentTransactionRecordsApp : AdminAppBuilder<PaymentTransaction, PaymentTransactionRecordsApp> {
		public override string Name { get { return "PaymentTransactionRecords"; } }
		public override string Url { get { return "/admin/payment_transactions"; } }
		public override string TileClass { get { return "tile bg-yellow-gold"; } }
		public override string IconClass { get { return "fa fa fa-download"; } }
		protected override IAjaxTableCallback<PaymentTransaction> GetTableCallback() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }
	}
}
