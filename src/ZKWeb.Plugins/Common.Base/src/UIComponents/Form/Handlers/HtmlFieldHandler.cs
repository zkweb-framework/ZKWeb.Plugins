using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Handlers {
	/// <summary>
	/// 直接显示Html内容的字段
	/// </summary>
	[ExportMany(ContractKey = typeof(HtmlFieldAttribute)), SingletonReuse]
	public class HtmlFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			return field.Value == null ? null : field.Value.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return null;
		}
	}
}
