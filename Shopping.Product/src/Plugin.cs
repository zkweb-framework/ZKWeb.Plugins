using DotLiquid;
using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.ProductStates;
using ZKWeb.Plugins.Shopping.Product.src.TypeTraits;
using ZKWeb.Templating.AreaSupport;

namespace ZKWeb.Plugins.Shopping.Product.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册商品状态的特征
			Application.Ioc.RegisterInstance(
				new ProductStateTrait() { VisibleFromProductList = true },
				serviceKey: typeof(OnSale));
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 商品详情页
			areaManager.GetArea("product_details").DefaultWidgets.Add("shopping.product.widgets/product_details");
			areaManager.GetArea("product_gallery").DefaultWidgets.Add("shopping.product.widgets/product_gallery");
			areaManager.GetArea("product_sales_info").DefaultWidgets.Add("shopping.product.widgets/product_sales_info");
			// 商品列表页
			var productListFilter = areaManager.GetArea("product_list_filter");
			var productListTable = areaManager.GetArea("product_list_table");
			productListFilter.DefaultWidgets.Add("shopping.product.widgets/product_filter_by_class");
			productListFilter.DefaultWidgets.Add("shopping.product.widgets/product_filter_by_tag");
			productListFilter.DefaultWidgets.Add("shopping.product.widgets/product_filter_by_price_and_order");
			productListTable.DefaultWidgets.Add("shopping.product.widgets/product_list_table");
		}
	}
}
