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
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldHandlers {
	/// <summary>
	/// 商品匹配数据编辑器
	/// </summary>
	[ExportMany(ContractKey = typeof(ProductMatchedDatasEditorAttribute)), SingletonReuse]
	public class ProductMatchedDataEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (ProductMatchedDatasEditorAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			var translations = new Dictionary<string, string>() {
				{ "Condition", new T("Condition") },
				{ "Default", new T("Default") }
			};
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			html.AddAttribute("class", "product-matched-data-editor");
			html.AddAttribute("data-toggle", "product-matched-data-editor");
			html.AddAttribute("data-category-id-name", attribute.CategoryFieldName);
			html.AddAttribute("data-matched-datas-name", attribute.Name);
			html.AddAttribute("data-table-class", "table table-bordered table-hover");
			html.AddAttribute("data-table-header-class", "heading");
			html.AddAttribute("data-translations", JsonConvert.SerializeObject(translations));
			html.RenderBeginTag("div");
			html.RenderEndTag();
			return html.InnerWriter.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<List<EditingMatchedData>>(value) ??
				new List<EditingMatchedData>();
		}
	}
}
