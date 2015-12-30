using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	[ExportMany(ContractKey = typeof(CaptchaFieldAttribute)), SingletonReuse]
	public class Captcha : IFormFieldHandler {
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			throw new NotImplementedException();
		}

		public object Parse(FormField field, string value) {
			throw new NotImplementedException();
		}
	}
}
