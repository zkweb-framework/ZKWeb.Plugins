using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.AdminSettings.src;
using ZKWeb.Plugins.Common.AdminSettings.src.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.ListItemProviders;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Localize;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Region.src.GenericFormsForAdminSettings {
	/// <summary>
	/// 地区设置
	/// </summary>
	[ExportMany]
	public class RegionSettingsForm : GenericFormForAdminSettings {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIcon { get { return "fa fa-wrench"; } }
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
