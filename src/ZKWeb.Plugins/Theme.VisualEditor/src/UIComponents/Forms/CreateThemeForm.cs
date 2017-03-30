using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Services;
using ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.UIComponents.Forms {
	/// <summary>
	/// 创建主题的表单
	/// </summary>
	[Form("UploadThemeForm", "/api/visual_editor/create_theme")]
	public class CreateThemeForm : ModelFormBuilder {
		/// <summary>
		/// 主题名称
		/// </summary>
		[Required]
		[TextBoxField("ThemeName", "ThemeName")]
		public string ThemeName { get; set; }
		/// <summary>
		/// 主题文件名
		/// </summary>
		[Required]
		[TextBoxField("ThemeFilename", "ThemeFilename")]
		public string ThemeFilename { get; set; }
		/// <summary>
		/// 主题描述
		/// </summary>
		[TextBoxField("ThemeDescription", "ThemeDescription")]
		public string ThemeDescription { get; set; }
		/// <summary>
		/// 主题作者
		/// </summary>
		[Required]
		[TextBoxField("ThemeAuthor", "ThemeAuthor")]
		public string ThemeAuthor { get; set; }
		/// <summary>
		/// 主题版本
		/// </summary>
		[Required]
		[TextBoxField("ThemeVersion", "ThemeVersion")]
		public string ThemeVersion { get; set; }
		/// <summary>
		/// 主题预览图
		/// </summary>
		[FileUploaderField("ThemePreviewImage")]
		public IHttpPostedFile ThemePreviewImage { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			ThemeVersion = "1.0";
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var themeInfo = new VisualThemeInfo();
			themeInfo.Name = ThemeName;
			themeInfo.Description = ThemeDescription;
			themeInfo.Author = ThemeAuthor;
			themeInfo.Version = ThemeVersion;
			themeInfo.Filename = ThemeFilename;
			var previewStream = ThemePreviewImage?.OpenReadStream();
			if (previewStream != null) {
				using (var image = Image.FromStream(previewStream))
				using (var memoryStream = new MemoryStream()) {
					image.Save(memoryStream, ImageFormat.Png);
					themeInfo.PreviewImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			var themeManager = Application.Ioc.Resolve<VisualThemeManager>();
			themeManager.CreateTheme(themeInfo);
			return this.SaveSuccessAndRefreshModal();
		}
	}
}
