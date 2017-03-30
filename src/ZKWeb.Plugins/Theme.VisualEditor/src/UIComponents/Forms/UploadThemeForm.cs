using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.UIComponents.Forms {
	/// <summary>
	/// 上传主题的表单
	/// </summary>
	[Form("UploadThemeForm", "/api/visual_editor/upload_theme")]
	public class UploadThemeForm : ModelFormBuilder {
		/// <summary>
		/// 主题文件
		/// </summary>
		[FileUploaderField("ThemeFile", "zip", 100 * 1024 * 1024)]
		public IHttpPostedFile ThemeFile { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.UploadTheme(ThemeFile.FileName, ThemeFile.OpenReadStream());
			return this.SaveSuccessAndRefreshModal();
		}
	}
}
