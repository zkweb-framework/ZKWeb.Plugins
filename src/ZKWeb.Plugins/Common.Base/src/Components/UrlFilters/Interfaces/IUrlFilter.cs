namespace ZKWeb.Plugins.Common.Base.src.Components.UrlFilters.Interfaces {
	/// <summary>
	/// 全局Url过滤器
	/// </summary>
	public interface IUrlFilter {
		/// <summary>
		/// 过滤Url
		/// </summary>
		/// <param name="url">url</param>
		void Filter(ref string url);
	}
}
