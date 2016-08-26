namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form {
	/// <summary>
	/// 从模型构建表单的构建器，支持多标签
	/// </summary>
	public abstract class TabModelFormBuilder :  ModelFormBuilder {
		/// <summary>
		/// 初始化
		/// </summary>
		public TabModelFormBuilder() : base(new TabFormBuilder()) { }
	}
}
