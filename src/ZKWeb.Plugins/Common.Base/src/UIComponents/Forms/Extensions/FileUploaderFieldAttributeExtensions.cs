using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions {
	/// <summary>
	/// 文件上传字段属性的扩展函数
	/// </summary>
	public static class FileUploaderFieldAttributeExtensions {
		/// <summary>
		/// 检查上传的文件
		/// </summary>
		/// <param name="attribute">上传属性</param>
		/// <param name="file">文件对象</param>
		public static void Check(this FileUploaderFieldAttribute attribute, IHttpPostedFile file) {
			if (file == null) {
				return;
			} else if (!attribute.Extensions.Contains(
				Path.GetExtension(file.FileName).Substring(1))) {
				// 检查后缀
				throw new Exception(string.Format(
					new T("Only {0} files are allowed"),
					string.Join(",", attribute.Extensions)));
			} else if (file.Length > attribute.MaxContentsLength) {
				// 检查大小
				throw new Exception(string.Format(
					new T("Please upload file size not greater than {0}"),
					FileUtils.GetSizeDisplayName((int)attribute.MaxContentsLength)));
			}
		}
	}
}
