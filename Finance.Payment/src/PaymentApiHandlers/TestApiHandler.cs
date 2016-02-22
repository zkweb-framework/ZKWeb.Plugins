using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;
using ZKWeb.Templating;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentApiHandlers {
	/// <summary>
	/// 测试接口的处理器
	/// </summary>
	[ExportMany]
	public class TestApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "TestApi"; } }
		/// <summary>
		/// 编辑中的接口数据
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
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var form = new TestApiPayForm(transaction);
			form.Bind();
			html = new HtmlString(templateManager.RenderTemplate("finance.payment/test_api_pay.html", new { form }));
		}

		/// <summary>
		/// 调用发货接口
		/// 发货后自动确认收货
		/// </summary>
		public void SendGoods(
			DatabaseContext context, PaymentTransaction transaction, string logisticsName, string invoiceNo) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format(
				"PaymentApi send goods: transaction {0} logisticsName {1} invoiceNo {2}",
				transaction.Serial, logisticsName, invoiceNo));
			var transactionRepository = RepositoryResolver.Resolve<PaymentTransactionRepository, PaymentTransaction>(context);
			transactionRepository.SendGoods(transaction.Id, logisticsName, invoiceNo);
		}

		/// <summary>
		/// 接口数据
		/// </summary>
		public class ApiData {
			/// <summary>
			/// 支付密码
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("PaymentPassword", "Password required to pay transactions")]
			public string PaymentPassword { get; set; }
		}

		/// <summary>
		/// 测试接口支付时使用的表单
		/// </summary>
		[Form("TestApiPayForm", "admin/payment_apis/test_api_pay")]
		public class TestApiPayForm : ModelFormBuilder {
			/// <summary>
			/// 需要支付的交易
			/// </summary>
			public PaymentTransaction Transaction { get; set; }
			/// <summary>
			/// 交易Id
			/// </summary>
			[HiddenField("Id")]
			public long Id { get; set; }
			/// <summary>
			/// 流水号
			/// </summary>
			[LabelField("Serial")]
			public string Serial { get; set; }
			/// <summary>
			/// 货币
			/// </summary>
			[LabelField("Currency")]
			public string Currency { get; set; }
			/// <summary>
			/// 金额
			/// </summary>
			[LabelField("Amount")]
			public string Amount { get; set; }
			/// <summary>
			/// 交易描述
			/// </summary>
			[LabelField("Description")]
			public string Description { get; set; }
			/// <summary>
			/// 支付密码
			/// </summary>
			[PasswordField("PaymentPassword", "PaymentPassword")]
			public string PaymentPassword { get; set; }
			/// <summary>
			/// 支付类型
			/// 即时到账或担保交易
			/// </summary>
			[TextBoxField("Action")]
			public string PayType { get; set; }

			/// <summary>
			/// 初始化
			/// 在这里会进行安全检查
			/// </summary>
			/// <param name="transaction">需要支付的交易</param>
			public TestApiPayForm(PaymentTransaction transaction) {
				// 检查交易对应的接口类型是否测试接口
				if (transaction.Api.Type != "TestApi") {
					throw new HttpException(400,
						new T("Use test api to pay transaction created with other api type is not allowed"));
				}
				// 检查当前登录用户是否可支付
				var result = transaction.Check(c => c.IsPayableByLoggedinUser);
				if (!result.Item1) {
					throw new HttpException(400, result.Item2);
				}
				Transaction = transaction;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
				var currency = currencyManager.GetCurrency(Transaction.CurrencyType);
				Id = Transaction.Id;
				Serial = Transaction.Serial;
				Currency = new T(currency.Type);
				Amount = currency.Format(Transaction.Amount);
				Description = Transaction.Description;
				PaymentPassword = null;
				PayType = null;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				throw new NotImplementedException();
			}
		}
	}
}
