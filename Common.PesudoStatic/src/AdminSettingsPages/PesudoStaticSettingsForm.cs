using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.PesudoStatic.src.Config;
using ZKWeb.Plugins.Common.PesudoStatic.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.AdminSettingsPages {
	/// <summary>
	/// 伪静态设置
	/// </summary>
	[ExportMany]
	public class PesudoStaticSettingsForm : AdminSettingsFormPageBuilder {
		public override string Group { get { return "BaseSettings"; } }
		public override string GroupIconClass { get { return "fa fa-wrench"; } }
		public override string Name { get { return "PesudoStaticSettings"; } }
		public override string IconClass { get { return "fa fa-file-code-o"; } }
		public override string Url { get { return "/admin/settings/pesudo_static_settings"; } }
		public override string Privilege { get { return "AdminSettings:PesudoStaticSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 启用伪静态
			/// </summary>
			[CheckBoxField("EnablePesudoStatic")]
			public bool EnablePesudoStatic { get; set; }
			/// <summary>
			/// 伪静态扩展名
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 2)]
			[TextBoxField("PesudoStaticExtension", "PesudoStaticExtension")]
			public string PesudoStaticExtension { get; set; }
			/// <summary>
			/// 伪静态参数分隔符
			/// </summary>
			[Required]
			[StringLength(1, MinimumLength = 1)]
			[TextBoxField("PesudoStaticParamDelimiter", "PesudoStaticParamDelimiter")]
			public string PesudoStaticParamDelimiter { get; set; }
			/// <summary>
			/// 伪静态策略
			/// </summary>
			[Required]
			[RadioButtonsField("PesudoStaticPolicy", typeof(ListItemFromEnum<PesudoStaticPolicies>))]
			public PesudoStaticPolicies PesudoStaticPolicy { get; set; }
			/// <summary>
			/// 包含的Url路径
			/// 一行一个，仅在白名单策略下生效
			/// </summary>
			[TextAreaField("IncludeUrlPaths", 5, "One path per line, only available for whitelist policy")]
			public string IncludeUrlPaths { get; set; }
			/// <summary>
			/// 排除的Url路径
			/// 一行一个，仅在黑名单策略下生效
			/// </summary>
			[TextAreaField("ExcludeUrlPaths", 5, "One path per line, only available for blacklist policy")]
			public string ExcludeUrlPaths { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<PesudoStaticSettings>();
				EnablePesudoStatic = settings.EnablePesudoStatic;
				PesudoStaticExtension = settings.PesudoStaticExtension;
				PesudoStaticParamDelimiter = settings.PesudoStaticParamDelimiter.ToString();
				PesudoStaticPolicy = settings.PesudoStaticPolicy;
				IncludeUrlPaths = string.Join("\r\n", settings.IncludeUrlPaths);
				ExcludeUrlPaths = string.Join("\r\n", settings.ExcludeUrlPaths);
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<PesudoStaticSettings>();
				settings.EnablePesudoStatic = EnablePesudoStatic;
				settings.PesudoStaticExtension = PesudoStaticExtension;
				if (!settings.PesudoStaticExtension.StartsWith(".")) {
					// 后缀名需要自动加上"."
					settings.PesudoStaticExtension = "." + settings.PesudoStaticExtension;
				}
				settings.PesudoStaticParamDelimiter = PesudoStaticParamDelimiter[0];
				settings.PesudoStaticPolicy = PesudoStaticPolicy;
				settings.IncludeUrlPaths.Clear();
				settings.IncludeUrlPaths.AddRange(IncludeUrlPaths.Split('\n')
					.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)));
				settings.ExcludeUrlPaths.Clear();
				settings.ExcludeUrlPaths.AddRange(ExcludeUrlPaths.Split('\n')
					.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)));
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
