using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.FormFieldAttributes;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Utils.Extensions;

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
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (RegionEditorAttribute)field.Attribute;
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = configManager.GetData<RegionSettings>();
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("require-script", "/static/common.region.js/region-editor.min.js");
			html.AddAttribute("require-style", "/static/common.region.css/region-editor.css");
			html.AddAttribute("class", "region-editor");
			html.AddAttribute("data-trigger", "region-editor");
			html.AddAttribute("data-display-country-dropdown", JsonConvert.SerializeObject(
				attribute.DisplayCountryDropdown ?? regionSettings.DisplayCountryDropdown));
			html.RenderBeginTag("div");
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag(); // input
			html.RenderEndTag(); // div
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<CountryAndRegion>(value) ?? new CountryAndRegion();
		}
	}
}
