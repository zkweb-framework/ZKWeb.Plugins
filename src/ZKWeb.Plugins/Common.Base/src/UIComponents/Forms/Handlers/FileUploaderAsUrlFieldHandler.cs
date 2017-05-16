using System;
using System.Collections.Generic;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 文件上传
	/// 自动保存文件并返回Url
	/// </summary>
	[ExportMany(ContractKey = typeof(FileUploaderAsUrlFieldAttribute)), SingletonReuse]
	public class FileUploaderAsUrlFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var file = templateManager.RenderTemplate("common.base/tmpl.form.file_as_url.html", new {
				name = field.Attribute.Name,
				attributes = htmlAttributes,
				value = field.Value
			});
			return field.WrapFieldHtml(htmlAttributes, file);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			// 获取原Url值
			var attribute = (FileUploaderAsUrlFieldAttribute)field.Attribute;
			var request = HttpManager.CurrentContext.Request;
			var url = request.Get<string>(attribute.Name + "_Url");
			// 获取删除勾选框，勾选时返回null
			var delete = request.Get<bool>(attribute.Name + "_Delete");
			if (delete) {
				return null;
			}
			// 获取上传的文件，不存在时返回原来的Url
			var file = request.GetPostedFile(attribute.Name);
			attribute.Check(file);
			if (file == null) {
				return url;
			}
			// 计算文件校验值
			byte[] bytes;
			using (var memoryStream = new MemoryStream())
			using (var uploadStream = file.OpenReadStream()) {
				uploadStream.CopyTo(memoryStream);
				bytes = memoryStream.ToArray();
			}
			var hash = PasswordUtils.Sha1Sum(bytes).ToHex();
			// 保存文件到本地
			var filename = hash + Path.GetExtension(file.FileName).ToLower();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var fileEntry = fileStorage.GetStorageFile("static", attribute.UploadDirectory, filename);
			using (var writeStream = fileEntry.OpenWrite()) {
				// TODO: zkweb 1.9改成WriteAllBytes
				writeStream.Write(bytes, 0, bytes.Length);
			}
			return $"/static/{attribute.UploadDirectory}/{filename}";
		}
	}
}
