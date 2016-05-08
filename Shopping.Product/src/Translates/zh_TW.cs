﻿using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Product", "商品" },
			{ "ProductManage", "商品管理" },
			{ "Product management for ec site", "商城使用的商品管理功能" },
			{ "ProductType", "商品類型" },
			{ "RealProduct", "實體商品" },
			{ "VirtualProduct", "虛擬商品" },
			{ "ProductState", "商品狀態" },
			{ "OnSale", "上架中" },
			{ "StopSelling", "已下架" },
			{ "WaitForSales", "準備上架" },
			{ "ProductClass", "商品分類" },
			{ "ProductTag", "商品標簽" },
			{ "Price", "價格" },
			{ "Stock", "庫存" },
			{ "Seller", "賣家" },
			{ "ProductAlbumSettings", "商品相冊設置" },
			{ "ProductSettings", "商品設置" },
			{ "OriginalImageWidth", "原圖寬度" },
			{ "OriginalImageHeight", "原圖高度" },
			{ "ThumbnailImageWidth", "縮略圖寬度" },
			{ "ThumbnailImageHeight", "縮略圖高度" },
			{ "ProductAlbum", "商品相冊" },
			{ "ProductProperties", "規格屬性" },
			{ "ProductPriceAndStock", "價格庫存" },
			{ "ProductIntroduction", "商品介紹" },
			{ "As Main Image", "設為主圖" },
			{ "Clear", "清除" },
			{ "Uploaded pictures will be scaled to [0]x[1], " +
				"upload pictures of this size can achieve the best display effect",
				"當前上傳的圖片會被縮放到[0]x[1]，上傳此大小的圖片可以達到最佳的顯示效果" },
			{ "Category", "類目" },
			{ "Category not exists", "類目不存在" },
			{ "Non sales properties", "屬性" },
			{ "Sales properties", "規格" },
			{ "Sure to change category? The properties you selected will lost!",
				"確認需要修改類目？當前選擇的規格和屬性將會丟失！"},
			{ "Condition", "條件" },
			{ "Default", "默認" },
			{ "Order count >=", "訂購數量 >=" },
			{ "InhertIfEmpty", "留空時繼承" },
			{ "PriceCurrency", "價格貨幣" },
			{ "OrderCountGE", "訂購數量 >=" },
			{ "All", "全部" },
			{ "Weight", "重量" },
			{ "Weight(g)", "重量(克)" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}