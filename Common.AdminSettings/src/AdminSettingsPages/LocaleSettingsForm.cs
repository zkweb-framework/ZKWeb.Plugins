using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using ZKWeb.Plugins.Common.AdminSettings.src.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.AdminSettings.src.AdminSettingsPages {
	/// <summary>
	/// 语言设置
	/// </summary>
	[ExportMany]
	public class LocaleSettingsForm : AdminSettingsFormPageBuilder {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIconClass { get { return "fa fa-wrench"; } }
		public override string Name { get { return "LocaleSettings"; } }
		public override string IconClass { get { return "fa fa-language"; } }
		public override string Url { get { return "/admin/settings/locale_settings"; } }
		public override string Privilege { get { return "AdminSettings:LocaleSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 默认语言
			/// </summary>
			[Required]
			[DropdownListField("DefaultLanguage", typeof(LanguageListItemProvider))]
			public string DefaultLanguage { get; set; }
			/// <summary>
			/// 默认时区
			/// </summary>
			[Required]
			[DropdownListField("DefaultTimezone", typeof(TimezoneListItemProvider))]
			public string DefaultTimezone { get; set; }
			/// <summary>
			/// 是否允许自动检测浏览器语言
			/// </summary>
			[CheckBoxField("AllowDetectLanguageFromBrowser")]
			public bool AllowDetectLanguageFromBrowser { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<LocaleSettings>();
				DefaultLanguage = settings.DefaultLanguage;
				DefaultTimezone = settings.DefaultTimezone;
				AllowDetectLanguageFromBrowser = settings.AllowDetectLanguageFromBrowser;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<LocaleSettings>();
				settings.DefaultLanguage = DefaultLanguage;
				settings.DefaultTimezone = DefaultTimezone;
				settings.AllowDetectLanguageFromBrowser = AllowDetectLanguageFromBrowser;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
