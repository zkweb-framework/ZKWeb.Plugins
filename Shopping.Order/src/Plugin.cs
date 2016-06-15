using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 商品详情页
			areaManager.GetArea("product_sales_info")
				.DefaultWidgets.Add("shopping.order.widgets/product_purchase_buttons");
			// 迷你购物车
			areaManager.GetArea("header_navbar_right")
				.DefaultWidgets.AddBefore("", "shopping.order.widgets/mini_cart_menu");
			// 购物车
			areaManager.GetArea("cart_contents")
				.DefaultWidgets.Add("shopping.order.widgets/cart_contents");
		}
	}
}
