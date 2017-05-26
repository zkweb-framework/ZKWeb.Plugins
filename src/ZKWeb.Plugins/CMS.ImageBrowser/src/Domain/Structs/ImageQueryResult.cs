namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Domain.Structs {
	/// <summary>
	/// 图片的查询结果
	/// </summary>
	public class ImageQueryResult {
		/// <summary>
		/// 类别
		/// </summary>
		public string Category { get; set; }
		/// <summary>
		/// 不包含后缀名的文件名
		/// </summary>
		public string FilenameWithoutExtension { get; set; }
		/// <summary>
		/// 后缀名
		/// </summary>
		public string Extension { get; set; }
	}
}
