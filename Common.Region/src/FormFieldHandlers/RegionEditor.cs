using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Region.src.FormFieldAttributes;

namespace ZKWeb.Plugins.Common.Region.src.FormFieldHandlers {
	/// <summary>
	/// 地区联动下拉框
	/// </summary>
	[ExportMany(ContractKey = typeof(RegionEditorAttribute)), SingletonReuse]
	public class RegionEditor : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			throw new NotImplementedException();
		}
	}
}
