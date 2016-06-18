using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.FormFieldAttributes;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.FormFieldHandlers {
	/// <summary>
	/// 地区联动下拉框
	/// 成员类型请使用CountryAndRegion
	/// </summary>
	[ExportMany(ContractKey = typeof(RegionEditorAttribute)), SingletonReuse]
	public class RegionEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (RegionEditorAttribute)field.Attribute;
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = configManager.GetData<RegionSettings>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var regionEditor = templateManager.RenderTemplate(
				"common.region/tmpl.region_editor.html", new {
					name = attribute.Name,
					value = JsonConvert.SerializeObject(field.Value),
					attributes = htmlAttributes,
					displayCountryDropdown = JsonConvert.SerializeObject(
					attribute.DisplayCountryDropdown ?? regionSettings.DisplayCountryDropdown)
				});
			return field.WrapFieldHtml(htmlAttributes, regionEditor);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return JsonConvert.DeserializeObject<CountryAndRegion>(values[0]) ?? new CountryAndRegion();
		}
	}
}
