using DotLiquid;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 下拉框或单选按钮使用的选项
	/// </summary>
	public class ListItem : ILiquidizable {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ListItem() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		public ListItem(string name, string value) {
			Name = name;
			Value = value;
		}

		/// <summary>
		/// 允许描画元素到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { name = Name, value = Value };
		}
	}
}
