namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 选项值 + 选项列表提供器
	/// 选项列表提供器根据字段类型不同
	/// 默认有以下类型的提供器
	/// - IListItemGroupsProvider
	/// - IListItemProvider
	/// - IListItemTreeProvider
	/// </summary>
	public class ListItemValueWithProvider {
		/// <summary>
		/// 选项值
		/// </summary>
		public object Value { get; set; }
		/// <summary>
		/// 选项列表提供器
		/// 解析提交内容时这里会等于null
		/// </summary>
		public object Provider { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ListItemValueWithProvider(object value, object provider) {
			Value = value;
			Provider = provider;
		}
	}
}
