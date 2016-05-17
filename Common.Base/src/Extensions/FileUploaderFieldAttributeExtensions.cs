using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// 文件上传字段属性的扩展函数
	/// </summary>
	public static class FileUploaderFieldAttributeExtensions {
		/// <summary>
		/// 检查上传的文件
		/// </summary>
		/// <param name="file">文件对象</param>
		public static void Check(this FileUploaderFieldAttribute attribute, HttpPostedFileBase file) {
			if (file == null) {
				return;
			} else if (!attribute.Extensions.Contains(
				Path.GetExtension(file.FileName).Substring(1))) {
				// 检查后缀
				throw new Exception(string.Format(
					new T("Only {0} files are allowed"),
					string.Join(",", attribute.Extensions)));
			} else if (file.ContentLength > attribute.MaxContentsLength) {
				// 检查大小
				throw new Exception(string.Format(
					new T("Please upload file size not greater than {0}"),
					FileUtils.GetSizeDisplayName((int)attribute.MaxContentsLength)));
			}
		}
	}
}
