using DryIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品图片上传器使用的数据
	/// </summary>
	public class ProductAlbumUploadData {
		/// <summary>
		/// 可上传的商品图片数量
		/// 全局变量
		/// </summary>
		public static int MaxImageCount = 5;
		/// <summary>
		/// 当前相册的图片列表
		/// 仅绑定时使用
		/// </summary>
		public List<string> ImageUrls { get; set; }
		/// <summary>
		/// 上传的图片列表
		/// 仅提交时使用
		/// </summary>
		public List<HttpPostedFileBase> UploadedImages { get; set; }
		/// <summary>
		/// 要删除的图片列表
		/// 仅提交时使用
		/// </summary>
		public List<bool> DeleteImages { get; set; }
		/// <summary>
		/// 主图序号
		/// 从1开始
		/// </summary>
		public long MainImageIndex { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductAlbumUploadData() {
			ImageUrls = new List<string>();
			UploadedImages = new List<HttpPostedFileBase>();
			DeleteImages = new List<bool>();
			MainImageIndex = 1;
		}

		/// <summary>
		/// 初始化，用于绑定
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <param name="maxIndex">最多检测到的序号</param>
		public ProductAlbumUploadData(long productId) : this() {
			// 获取当前相册的图片列表
			var albumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			for (int x = 1; x <= MaxImageCount; ++x) {
				ImageUrls.Add(albumManager.GetAlbumImageWebPath(productId, x, ProductAlbumImageType.Thumbnail));
			}
			// 获取主图路径，不存在时返回
			var mainPath = albumManager.GetAlbumImageStoragePath(productId, null, ProductAlbumImageType.Normal);
			var mainFileInfo = new FileInfo(mainPath);
			if (!mainFileInfo.Exists) {
				return;
			}
			// 判断相册中哪张图片的大小和修改时间和原图一致
			// 没有时默认选择第一张
			for (int x = 1; x <= MaxImageCount; ++x) {
				var path = albumManager.GetAlbumImageStoragePath(productId, x, ProductAlbumImageType.Normal);
				var fileInfo = new FileInfo(path);
				if (fileInfo.Exists && fileInfo.Length == mainFileInfo.Length &&
					fileInfo.LastWriteTimeUtc.Truncate() == mainFileInfo.LastWriteTimeUtc.Truncate()) {
					MainImageIndex = x;
					return;
				}
			}
			MainImageIndex = 1;
		}

		/// <summary>
		/// 保存文件
		/// </summary>
		/// <param name="productId">商品Id</param>
		public void SaveFiles(long productId) {
			// 保存图片
			var albumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			for (int x = 1; x <= MaxImageCount; ++x) {
				var image = UploadedImages[x - 1];
				var delete = DeleteImages[x - 1];
				if (delete) {
					albumManager.DeleteAlbumImage(productId, x);
				} else if (image != null) {
					albumManager.SaveAlbumImage(productId, x, image.InputStream);
				}
			}
			// 设置主图
			// 序号对应的图片不存在时设置第一张存在的图为主图
			// 否则删除主图
			if (albumManager.SetMainAlbumImage(productId, MainImageIndex)) {
			} else if (Enumerable.Range(1, MaxImageCount).Any(
				x => albumManager.SetMainAlbumImage(productId, x))) {
			} else {
				albumManager.DeleteAlbumImage(productId, null);
			} 
		}
	}
}
