using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;
using ZKWeb.Plugins.Shopping.Logistics.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Region.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Structs;
using ZKWeb.Plugins.Common.Currency.src.UIComponents.ListItemProviders;

namespace ZKWeb.Plugins.Shopping.Logistics.src.UIComponents.FormFieldHandlers {
	/// <summary>
	/// 运费规则编辑器
	/// </summary>
	[ExportMany(ContractKey = typeof(LogisticsPriceRulesEditorAttribute)), SingletonReuse]
	public class LogisticsPriceRulesEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (LogisticsPriceRulesEditorAttribute)field.Attribute;
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = configManager.GetData<RegionSettings>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
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
			return templateManager.RenderTemplate("shopping.logistics/tmpl.price_rules_editor.html", new {
				name = field.Attribute.Name,
				value = JsonConvert.SerializeObject(field.Value),
				attributes = htmlAttributes,
				displayCountryDropdown = JsonConvert.SerializeObject(
					attribute.DisplayCountryDropdown ?? regionSettings.DisplayCountryDropdown),
				currencyListItems = JsonConvert.SerializeObject(
					new CurrencyListItemProvider().GetItems().ToList()),
				defaultPriceRule = JsonConvert.SerializeObject(PriceRule.GetDefault()),
				translations = JsonConvert.SerializeObject(translations)
			});
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return JsonConvert.DeserializeObject<List<PriceRule>>(values[0]) ?? new List<PriceRule>();
		}
	}
}
