using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.Components.GenericConfigs;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品相册设置
	/// </summary>
	[ExportMany]
	public class ProductAlbumSettingsController : FormAdminSettingsControllerBase {
		public override string Group { get { return "ProductSettings"; } }
		public override string GroupIconClass { get { return "fa fa-diamond"; } }
		public override string Name { get { return "ProductAlbumSettings"; } }
		public override string IconClass { get { return "fa fa-image"; } }
		public override string Url { get { return "/admin/settings/product_album_settings"; } }
		public override string Privilege { get { return "AdminSettings:ProductAlbumSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 原图宽度
			/// </summary>
			[Required]
			[TextBoxField("OriginalImageWidth", "OriginalImageWidth")]
			public long OriginalImageWidth { get; set; }
			/// <summary>
			/// 原图高度
			/// </summary>
			[Required]
			[TextBoxField("OriginalImageHeight", "OriginalImageHeight")]
			public long OriginalImageHeight { get; set; }
			/// <summary>
			/// 缩略图宽度
			/// </summary>
			[Required]
			[TextBoxField("ThumbnailImageWidth", "ThumbnailImageWidth")]
			public long ThumbnailImageWidth { get; set; }
			/// <summary>
			/// 缩略图高度
			/// </summary>
			[Required]
			[TextBoxField("ThumbnailImageHeight", "ThumbnailImageHeight")]
			public long ThumbnailImageHeight { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ProductAlbumSettings>();
				OriginalImageWidth = settings.OriginalImageWidth;
				OriginalImageHeight = settings.OriginalImageHeight;
				ThumbnailImageWidth = settings.ThumbnailImageWidth;
				ThumbnailImageHeight = settings.ThumbnailImageHeight;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ProductAlbumSettings>();
				settings.OriginalImageWidth = OriginalImageWidth;
				settings.OriginalImageHeight = OriginalImageHeight;
				settings.ThumbnailImageWidth = ThumbnailImageWidth;
				settings.ThumbnailImageHeight = ThumbnailImageHeight;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
