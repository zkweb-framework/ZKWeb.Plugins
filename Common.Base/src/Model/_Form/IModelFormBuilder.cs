namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 从模型构建表单的构建器的接口
	/// </summary>
	public interface IModelFormBuilder {
		/// <summary>
		/// 绑定表单
		/// </summary>
		void Bind();
		/// <summary>
		/// 提交表单，返回处理结果
		/// </summary>
		object Submit();
		/// <summary>
		/// 获取表单属性
		/// </summary>
		FormAttribute GetFormAttribute();
	}
}
