using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.AdminSettings.src.GenericFormsForAdminSettings {
	/// <summary>
	/// 网站设置
	/// </summary>
	[ExportMany]
	public class WebsiteSettingsForm : GenericFormForAdminSettings {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIcon { get { return "fa fa-wrench"; } }
		public override string Name { get { return "WebsiteSettings"; } }
		public override string IconClass { get { return "fa fa-globe"; } }
		public override string Url { get { return "/admin/settings/website_settings"; } }
		public override string Privilege { get { return "AdminSettings:WebsiteSettings"; } }
		public override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 网站名称
			/// </summary>
			[Required]
			[TextBoxField("WebsiteName", "WebsiteName")]
			public string WebsiteName { get; set; }
			/// <summary>
			/// 网页标题的默认格式
			/// </summary>
			[Required]
			[TextBoxField("DocumentTitleFormat", "Default is {title} - {websiteName}")]
			public string DocumentTitleFormat { get; set; }
			/// <summary>
			/// 版权信息
			/// </summary>
			[TextBoxField("CopyrightText", "CopyrightText")]
			public string CopyrightText { get; set; }

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<WebsiteSettings>();
				WebsiteName = settings.WebsiteName;
				DocumentTitleFormat = settings.DocumentTitleFormat;
				CopyrightText = settings.CopyrightText;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<WebsiteSettings>();
				settings.WebsiteName = WebsiteName;
				settings.DocumentTitleFormat = DocumentTitleFormat;
				settings.CopyrightText = CopyrightText;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
