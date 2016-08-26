namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form {
	/// <summary>
	/// 只包含字段内容的表单构建器
	/// </summary>
	public abstract class FieldsOnlyModelFormBuilder : ModelFormBuilder {
		/// <summary>
		/// 初始化
		/// </summary>
		public FieldsOnlyModelFormBuilder() : base(new FieldsOnlyFormBuilder()) { }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() { return null; }
	}
}
