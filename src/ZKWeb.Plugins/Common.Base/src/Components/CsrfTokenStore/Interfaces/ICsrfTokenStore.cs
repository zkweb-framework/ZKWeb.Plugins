namespace ZKWeb.Plugins.Common.Base.src.Components.CsrfTokenStore.Interfaces {
	/// <summary>
	/// CSRF校验的储存接口
	/// </summary>
	public interface ICsrfTokenStore {
		/// <summary>
		/// 获取CSRF校验
		/// </summary>
		/// <returns></returns>
		string GetCsrfToken();

		/// <summary>
		/// 设置CSRF校验
		/// </summary>
		/// <param name="token">CSRF校验</param>
		void SetCsrfToken(string token);

		/// <summary>
		/// 删除CSRF校验
		/// </summary>
		void RemoveCsrfToken();
	}
}
