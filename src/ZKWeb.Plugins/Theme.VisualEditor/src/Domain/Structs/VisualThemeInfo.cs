using DotLiquid;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Domain.Structs {
	/// <summary>
	/// 可视化主题信息
	/// </summary>
	public class VisualThemeInfo : ILiquidizable {
		/// <summary>
		/// 主题名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 主题描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 主题作者
		/// </summary>
		public string Author { get; set; }
		/// <summary>
		/// 主题版本
		/// </summary>
		public string Version { get; set; }
		/// <summary>
		/// 主题文件名
		/// 读取时设置
		/// </summary>
		public string Filename { get; set; }
		/// <summary>
		/// 预览图的Base64
		/// 读取时设置
		/// </summary>
		public string PreviewImageBase64 { get; set; }

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			return new { Name, Description, Author, Version, Filename, PreviewImageBase64 };
		}

		/// <summary>
		/// 设置预览图
		/// </summary>
		/// <param name="image">预览图</param>
		void SetPreviewImage(Image image) {
			using (var memoryStream = new MemoryStream()) {
				image.Save(memoryStream, ImageFormat.Png);
				memoryStream.Seek(0, SeekOrigin.Begin);
				PreviewImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
			}
		}
	}
}
