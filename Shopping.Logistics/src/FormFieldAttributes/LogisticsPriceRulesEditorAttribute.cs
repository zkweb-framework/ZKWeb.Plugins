using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Shopping.Logistics.src.FormFieldAttributes {
	/// <summary>
	/// 运费规则编辑器的属性
	/// </summary>
	public class LogisticsPriceRulesEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 是否显示国家下拉框，null时使用默认设置
		/// </summary>
		public bool? DisplayCountryDropdown { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public LogisticsPriceRulesEditorAttribute(string name) {
			Name = name;
			DisplayCountryDropdown = null;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="displayCountryDropdown">是否显示国家下拉框</param>
		public LogisticsPriceRulesEditorAttribute(string name, bool displayCountryDropdown) {
			Name = name;
			DisplayCountryDropdown = displayCountryDropdown;
		}
	}
}
