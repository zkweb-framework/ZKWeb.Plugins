namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 文件上传, 以url的形式自动保存
	/// </summary>
	public class FileUploaderAsUrlFieldAttribute : FileUploaderFieldAttribute {
		/// <summary>
		/// 上传目录
		/// </summary>
		public string UploadDirectory { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public FileUploaderAsUrlFieldAttribute(
			string name, string extensions = null, int maxContentsLength = 0, string uploadDirectory = null) :
			base(name, extensions, maxContentsLength) {
			UploadDirectory = uploadDirectory ?? "common.base.files";
		}
	}
}
