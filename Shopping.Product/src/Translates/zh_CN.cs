using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Product", "商品" },
			{ "ProductManage", "商品管理" },
			{ "Product management for ec site", "商城使用的商品管理功能" },
			{ "ProductType", "商品类型" },
			{ "RealProduct", "实体商品" },
			{ "VirtualProduct", "虚拟商品" },
			{ "ProductState", "商品状态" },
			{ "OnSale", "上架中" },
			{ "StopSelling", "已下架" },
			{ "WaitForSales", "准备上架" },
			{ "ProductClass", "商品分类" },
			{ "ProductTag", "商品标签" },
			{ "Price", "价格" },
			{ "Stock", "库存" },
			{ "Seller", "卖家" },
			{ "ProductAlbumSettings", "商品相册设置" },
			{ "ProductSettings", "商品设置" },
			{ "OriginalImageWidth", "原图宽度" },
			{ "OriginalImageHeight", "原图高度" },
			{ "ThumbnailImageWidth", "缩略图宽度" },
			{ "ThumbnailImageHeight", "缩略图高度" },
			{ "ProductAlbum", "商品相册" },
			{ "ProductProperties", "规格属性" },
			{ "ProductPriceAndStock", "价格库存" },
			{ "ProductIntroduction", "商品介绍" },
			{ "As Main Image", "设为主图" },
			{ "Clear", "清除" },
			{ "Uploaded pictures will be scaled to [0]x[1], " +
				"upload pictures of this size can achieve the best display effect",
				"当前上传的图片会被缩放到[0]x[1]，上传此大小的图片可以达到最佳的显示效果" },
			{ "Category", "类目" },
			{ "Category not exists", "类目不存在" },
			{ "Non sales properties", "属性" },
			{ "Sales properties", "规格" },
			{ "Sure to change category? The properties you selected will lost!",
				"确认需要修改类目？当前选择的规格和属性将会丢失！"},
			{ "Condition", "条件" },
			{ "Default", "默认" },
			{ "Order count >=", "订购数量 >=" },
			{ "InhertIfEmpty", "留空时继承" },
			{ "PriceCurrency", "价格货币" },
			{ "OrderCountGE", "订购数量 >=" },
			{ "All", "全部" },
			{ "Weight", "重量" },
			{ "Weight(g)", "重量(克)" },
			// TODO: 以下未复制到其他语言
			{ "ProductList", "商品列表" },
			{ "Preview", "预览" },
			{ "The product you are visiting does not exist.", "您查看的商品不存在" },
			{ "Brand", "品牌" },
			{ "ModelNumber", "货号" },
			{ "{0:F2} gram", "{0:F2} 克(g)" },
			{ "{0:F2}~{1:F2} gram", "{0:F2}~{1:F2} 克(g)" },
			{ "BestSales", "最佳销量" },
			{ "LowerPrice", "更低价格" },
			{ "HigherPrice", "更高价格" },
			{ "NewestOnSale", "最新上架" },
			{ "FilterByPrice", "按价格过滤" },
			{ "ProductListSettings", "商品列表设置" },
			{ "ProductsPerPage", "每页显示的商品数量" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
