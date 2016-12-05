using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders {
	using Product = Domain.Entities.Product;

	/// <summary>
	/// 获取货号的描述
	/// </summary>
	[ExportMany]
	public class ItemNoDescriptionProvider : IProductMatchParametersDescriptionProvider {
		/// <summary>
		/// 显示顺序
		/// </summary>
		public int DisplayOrder { get { return 200; } }

		/// <summary>
		/// 获取描述，没有时返回null
		/// </summary>
		public string GetDescription(Product product, ProductMatchParameters parameters) {
			var data = product.MatchedDatas
				.Where(d => !string.IsNullOrEmpty(d.ItemNo))
				.WhereMatched(parameters).FirstOrDefault();
			if (data != null) {
				return new T("ItemNo: {0}", data.ItemNo);
			}
			return null;
		}
	}
}
