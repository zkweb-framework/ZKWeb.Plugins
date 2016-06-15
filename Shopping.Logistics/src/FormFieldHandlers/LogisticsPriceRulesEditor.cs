using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Currency.src.ListItemProviders;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Shopping.Logistics.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.FormFieldHandlers {
	/// <summary>
	/// 运费规则编辑器
	/// </summary>
	[ExportMany(ContractKey = typeof(LogisticsPriceRulesEditorAttribute)), SingletonReuse]
	public class LogisticsPriceRulesEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (LogisticsPriceRulesEditorAttribute)field.Attribute;
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = configManager.GetData<RegionSettings>();
			var html = new HtmlTextWriter(new StringWriter());
			var translations = new Dictionary<string, string>() {
				{ "Default", new T("Default") },
				{ "Region", new T("Region") },
				{ "FirstHeavyUnit(g)", new T("FirstHeavyUnit(g)") },
				{ "FirstHeavyCost", new T("FirstHeavyCost") },
				{ "ContinuedHeavyUnit(g)", new T("ContinuedHeavyUnit(g)") },
				{ "ContinuedHeavyCost", new T("ContinuedHeavyCost") },
				{ "Currency", new T("Currency") },
				{ "Disabled", new T("Disabled") }
			};
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			html.AddAttribute("class", "logistics-price-rules-editor table-scroller");
			html.AddAttribute("data-toggle", "logistics-price-rules-editor");
			html.AddAttribute("data-price-rules-name", field.Attribute.Name);
			html.AddAttribute("data-table-class",
				 "table table-bordered table-hover table-editable table-editable-always-keep-last-row");
			html.AddAttribute("data-table-header-class", "heading");
			html.AddAttribute("data-display-country-dropdown", JsonConvert.SerializeObject(
				attribute.DisplayCountryDropdown ?? regionSettings.DisplayCountryDropdown));
			html.AddAttribute("data-currency-list-items", JsonConvert.SerializeObject(
				new CurrencyListItemProvider().GetItems().ToList()));
			html.AddAttribute("data-default-price-rule", JsonConvert.SerializeObject(
				PriceRule.GetDefault()));
			html.AddAttribute("data-translations", JsonConvert.SerializeObject(translations));
			html.RenderBeginTag("div");
			html.RenderEndTag();
			return html.InnerWriter.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<List<PriceRule>>(value) ?? new List<PriceRule>();
		}
	}
}
