using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Region.src.FormFieldAttributes {
	/// <summary>
	/// 地区联动下拉框的属性
	/// </summary>
	public class RegionEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 国家的字段名称，不传入时不显示国家下拉框
		/// </summary>
		public string CountryFieldName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="countryFieldName">国家的字段名称，不传入时不显示国家下拉框</param>
		public RegionEditorAttribute(string name, string countryFieldName = null) {
			Name = name;
			CountryFieldName = countryFieldName;
		}
	}
}
