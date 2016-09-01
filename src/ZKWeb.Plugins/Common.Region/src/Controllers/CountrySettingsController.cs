using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Region.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Region.src.UIComponents.ListItemProviders;

namespace ZKWeb.Plugins.Common.Region.src.Controllers {
	/// <summary>
	/// 地区设置
	/// </summary>
	[ExportMany]
	public class CountrySettingsController : FormAdminSettingsControllerBase {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIconClass { get { return "fa fa-wrench"; } }
		public override string Name { get { return "RegionSettings"; } }
		public override string IconClass { get { return "fa fa-flag"; } }
		public override string Url { get { return "/admin/settings/region_settings"; } }
		public override string Privilege { get { return "AdminSettings:RegionSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 默认国家/行政区
			/// </summary>
			[Required]
			[DropdownListField("DefaultCountry", typeof(CountryListItemProvider))]
			public string DefaultCountry { get; set; }
			/// <summary>
			/// 显示国家下拉框
			/// </summary>
			[CheckBoxField("DisplayCountryDropdown")]
			public bool DisplayCountryDropdown { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<RegionSettings>();
				DefaultCountry = settings.DefaultCountry;
				DisplayCountryDropdown = settings.DisplayCountryDropdown;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<RegionSettings>();
				settings.DefaultCountry = DefaultCountry;
				settings.DisplayCountryDropdown = DisplayCountryDropdown;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
