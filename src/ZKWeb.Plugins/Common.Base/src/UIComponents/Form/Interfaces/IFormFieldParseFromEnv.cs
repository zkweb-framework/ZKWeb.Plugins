namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Interfaces {
	/// <summary>
	/// 标记字段需要从提交环境中解析值，当表单值等于空时仍执行解析函数
	/// 一般用于文件上传的属性，例如FileUploaderFieldAttribute
	/// </summary>
	public interface IFormFieldParseFromEnv { }
}
