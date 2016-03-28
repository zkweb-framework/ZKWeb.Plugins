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
			{ "ProductIntroduction", "商品介绍" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
