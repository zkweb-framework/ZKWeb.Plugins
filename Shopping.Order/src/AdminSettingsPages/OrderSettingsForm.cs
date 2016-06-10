using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.AdminSettingsPages {
	/// <summary>
	/// 订单设置
	/// </summary>
	[ExportMany]
	public class OrderSettingsForm : AdminSettingsFormPageBuilder {
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
