using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.AdminSettings.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.LanguageSwitcher.src.Components.GenericConfigs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src.AdminSettingsPages {
	/// <summary>
	/// 语言切换设置的编辑页
	/// </summary>
	[ExportMany]
	public class LanguageSwitcherSettingsForm : FormAdminSettingsControllerBase {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIconClass { get { return "fa fa-wrench"; } }
		public override string Name { get { return "LanguageSwitcherSettings"; } }
		public override string IconClass { get { return "fa fa-font"; } }
		public override string Url { get { return "/admin/settings/language_switcher_settings"; } }
		public override string Privilege { get { return "AdminSettings:LanguageSwitcherSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 可以提供切换的语言列表
			/// </summary>
			[Required]
			[CheckBoxGroupField("SwitchableLanguages", typeof(LanguageListItemProvider))]
			public List<string> SwitchableLanguages { get; set; }
			/// <summary>
			/// 在前台页面显示语言切换器
			/// </summary>
			[CheckBoxField("DisplayLanguageSwitcherOnFrontPages")]
			public bool DisplaySwitcherOnFrontPages { get; set; }
			/// <summary>
			/// 在后台页面显示语言切换器
			/// </summary>
			[CheckBoxField("DisplayLanguageSwitcherOnAdminPanel")]
			public bool DisplaySwitcherOnAdminPanel { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<LanguageSwitcherSettings>();
				SwitchableLanguages = settings.SwitchableLanguages;
				DisplaySwitcherOnFrontPages = settings.DisplaySwitcherOnFrontPages;
				DisplaySwitcherOnAdminPanel = settings.DisplaySwitcherOnAdminPanel;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<LanguageSwitcherSettings>();
				settings.SwitchableLanguages = SwitchableLanguages ?? new List<string>();
				settings.DisplaySwitcherOnFrontPages = DisplaySwitcherOnFrontPages;
				settings.DisplaySwitcherOnAdminPanel = DisplaySwitcherOnAdminPanel;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
