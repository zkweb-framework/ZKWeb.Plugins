using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 文件上传
	/// </summary>
	public class FileUploaderFieldAttribute :
		FormFieldAttribute, IFormFieldRequireMultiPart, IFormFieldParseFromEnv {
		/// <summary>
		/// 允许的文件后缀
		/// </summary>
		public HashSet<string> Extensions { get; set; }
		/// <summary>
		/// 允许上传的最大长度，单位是字节
		/// </summary>
		public long MaxContentsLength { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段类型</param>
		/// <param name="extensions">允许的文件后缀，默认是图片后缀</param>
		/// <param name="maxContentsLength">允许上传的最大长度，单位是字节，默认是1MB</param>
		public FileUploaderFieldAttribute(
			string name, string extensions = null, int maxContentsLength = 0) {
			Name = name;
			Extensions = new HashSet<string>((extensions ?? "png,jpg,jpeg,gif").Split(','));
			MaxContentsLength = (maxContentsLength > 0) ? maxContentsLength : 1048576;
		}
	}
}
