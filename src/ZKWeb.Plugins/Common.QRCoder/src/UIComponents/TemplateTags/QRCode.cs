using System.IO;
using DotLiquid;
using System;
using QRCoder;
using System.Collections.Generic;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.QRCoder.src.UIComponents.TemplateTags {
	/// <summary>
	/// 二维码
	/// </summary>
	/// <example>
	/// {% qrcode "/login" %}
	/// {% qrcode "/login", "Q" %}
	/// {% qrcode "/login", "Q", 20 %}
	/// </example>
	public class QRCode : Tag {
		/// <summary>
		/// 图片的Html格式
		/// </summary>
		public const string ImgHtmlFormat = "<img src='data:image/png;base64,{0}' alt='' style='width: 100%; height: 100%;' />";
		/// <summary>
		/// 默认的二维码密度
		/// </summary>
		public const int DefaultPixelsPerModule = 20;
		/// <summary>
		/// Url地址
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// ECC等级
		/// </summary>
		public string ECCLevel { get; set; }
		/// <summary>
		/// 二维码密度
		/// </summary>
		public string PixelsPerModule { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public override void Initialize(string tagName, string markup, List<string> tokens) {
			base.Initialize(tagName, markup, tokens);
			var args = markup.Split(',');
			Url = args[0].Trim();
			ECCLevel = args.Length > 1 ? args[1].Trim() : "\"Q\"";
			PixelsPerModule = args.Length > 2 ? args[2].Trim() : $"\"{DefaultPixelsPerModule}\"";
		}

		/// <summary>
		/// 描画二维码图片
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var url = (context[Url] ?? "").ToString();
			var eccLevel = (context[ECCLevel] ?? "").ConvertOrDefault<QRCodeGenerator.ECCLevel>();
			var pixelsPerModule = (context[PixelsPerModule] ?? "").ConvertOrDefault<int>(DefaultPixelsPerModule);
			if (string.IsNullOrEmpty(url)) {
				throw new ArgumentNullException("QRCode url can't be empty");
			}
			var generator = new QRCodeGenerator();
			var qrCodeData = generator.CreateQrCode(url, eccLevel);
			var qrCode = new Base64QRCode(qrCodeData);
			var qrCodeBase64 = qrCode.GetGraphic(pixelsPerModule);
			result.WriteLine(string.Format(ImgHtmlFormat, qrCodeBase64));
		}
	}
}
