using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Html项
	/// 这个类型用于支持List[HtmlItem]的扩展函数
	/// </summary>
	public class HtmlItem : HtmlString {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="value"></param>
		public HtmlItem(string value) : base(value) { }
	}
}
