namespace ZKWeb.Plugins.Common.PesudoStatic.src.Model {
	/// <summary>
	/// 伪静态策略
	/// </summary>
	public enum PesudoStaticPolicies {
		/// <summary>
		/// 黑名单策略
		/// 除了排除的地址以外都启用伪静态
		/// </summary>
		BlackListPolicy = 0,
		/// <summary>
		/// 白名单策略
		/// 只有包含的地址启用伪静态
		/// </summary>
		WhiteListPolicy = 1
	}
}
