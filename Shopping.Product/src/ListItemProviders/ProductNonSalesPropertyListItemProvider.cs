using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Managers;

namespace ZKWeb.Plugins.Shopping.Product.src.ListItemProviders {
	/// <summary>
	/// 商品非销售属性的选项列表
	/// </summary>
	public class ProductNonSalesPropertyListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			foreach (var property in categoryManager.GetPropertyList()) {
				if (!property.IsSalesProperty) {
					yield return new ListItem(new T(property.Name), property.Id.ToString());
				}
			}
		}
	}
}
