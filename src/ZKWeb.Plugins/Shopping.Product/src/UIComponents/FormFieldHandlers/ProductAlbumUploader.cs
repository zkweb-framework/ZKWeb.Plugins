using System.Collections.Generic;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldHandlers {
	/// <summary>
	/// 商品相册上传器
	/// 编辑商品时使用
	/// </summary>
	[ExportMany(ContractKey = typeof(ProductAlbumUploaderAttribute)), SingletonReuse]
	public class ProductAlbumUploader : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (ProductAlbumUploaderAttribute)field.Attribute;
			var html = new StringBuilder();
			var value = (ProductAlbumUploadData)field.Value;
			var templateManger = Application.Ioc.Resolve<TemplateManager>();
			var albumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			for (int x = 1; x <= ProductAlbumUploadData.MaxImageCount; ++x) {
				html.Append(templateManger.RenderTemplate("shopping.product/tmpl.album_uploader.html", new {
					prefix = attribute.Name,
					image_url = value.ImageUrls[x - 1],
					index = x,
					is_main_image = value.MainImageIndex == x
				}));
			}
			return html.ToString();
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var data = new ProductAlbumUploadData();
			var attribute = (ProductAlbumUploaderAttribute)field.Attribute;
			var request = HttpManager.CurrentContext.Request;
			data.MainImageIndex = request.Get<long>(attribute.Name + "_MainImageIndex");
			for (int x = 1; x <= ProductAlbumUploadData.MaxImageCount; ++x) {
				var image = request.GetPostedFile(attribute.Name + "_Image_" + x);
				var deleteImage = request.Get<bool>(attribute.Name + "_DeleteImage_" + x);
				attribute.Check(image);
				data.UploadedImages.Add(image);
				data.DeleteImages.Add(deleteImage);
			}
			return data;
		}
	}
}
