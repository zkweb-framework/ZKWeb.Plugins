using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单设置
	/// </summary>
	[ExportMany]
	public class OrderSettingsController : FormAdminSettingsControllerBase {
		public override string Group { get { return "OrderSettings"; } }
		public override string GroupIconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string Name { get { return "OrderSettings"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string Url { get { return "/admin/settings/order_settings"; } }
		public override string Privilege { get { return "AdminSettings:OrderSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 立刻购买的购物车商品的过期天数
			/// </summary>
			[Required]
			[TextBoxField("BuynowCartProductExpiresDays", "BuynowCartProductExpiresDays")]
			public int BuynowCartProductExpiresDays { get; set; }
			/// <summary>
			/// 一般的购物车商品的过期天数
			/// </summary>
			[Required]
			[TextBoxField("NormalCartProductExpiresDays", "NormalCartProductExpiresDays")]
			public int NormalCartProductExpiresDays { get; set; }
			/// <summary>
			/// 自动确认收货天数
			/// </summary>
			[Required]
			[TextBoxField("AutoConfirmOrderAfterDays", "AutoConfirmOrderAfterDays")]
			public int AutoConfirmOrderAfterDays { get; set; }
			/// <summary>
			/// 允许非会员下单
			/// </summary>
			[CheckBoxField("AllowAnonymousVisitorCreateOrder")]
			public bool AllowAnonymousVisitorCreateOrder { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<OrderSettings>();
				BuynowCartProductExpiresDays = settings.BuynowCartProductExpiresDays;
				NormalCartProductExpiresDays = settings.NormalCartProductExpiresDays;
				AutoConfirmOrderAfterDays = settings.AutoConfirmOrderAfterDays;
				AllowAnonymousVisitorCreateOrder = settings.AllowAnonymousVisitorCreateOrder;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<OrderSettings>();
				settings.BuynowCartProductExpiresDays = BuynowCartProductExpiresDays;
				settings.NormalCartProductExpiresDays = NormalCartProductExpiresDays;
				settings.AutoConfirmOrderAfterDays = AutoConfirmOrderAfterDays;
				settings.AllowAnonymousVisitorCreateOrder = AllowAnonymousVisitorCreateOrder;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
