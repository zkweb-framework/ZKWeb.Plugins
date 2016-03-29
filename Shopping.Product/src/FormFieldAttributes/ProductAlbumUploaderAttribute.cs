using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes {
	/// <summary>
	/// 商品相册上传器的属性
	/// </summary>
	public class ProductAlbumUploaderAttribute : FormFieldAttribute,
		IFormFieldParseFromEnv, IFormFieldRequireMultiPart {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public ProductAlbumUploaderAttribute(string name) {
			Name = name;
		}
	}
}
