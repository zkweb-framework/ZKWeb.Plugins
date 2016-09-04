using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities {
	/// <summary>
	/// 自定义翻译
	/// 这个实体会保存到文件而不是数据库中
	/// </summary>
	public class CustomTranslation : IEntity<string> {
		/// <summary>
		/// 原文
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 译文
		/// </summary>
		public string Translated { get; set; }

		/// <summary>
		/// 显示原文
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Id;
		}
	}
}
