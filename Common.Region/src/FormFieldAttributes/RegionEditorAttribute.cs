using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Region.src.FormFieldAttributes {
	/// <summary>
	/// 地区联动下拉框的属性
	/// 类型请使用 CountryAndRegion
	/// </summary>
	public class RegionEditorAttribute : FormFieldAttribute {
		/// <summary>
		/// 是否显示国家下拉框，null时使用默认设置
		/// </summary>
		public bool? DisplayCountryDropdown { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public RegionEditorAttribute(string name) {
			Name = name;
			DisplayCountryDropdown = null;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="displayCountryDropdown">是否显示国家下拉框</param>
		public RegionEditorAttribute(string name, bool displayCountryDropdown) {
			Name = name;
			DisplayCountryDropdown = displayCountryDropdown;
		}
	}
}
