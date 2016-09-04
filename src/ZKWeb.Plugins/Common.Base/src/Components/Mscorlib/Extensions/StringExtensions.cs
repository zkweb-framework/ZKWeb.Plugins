namespace ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions {
	/// <summary>
	/// 字符串的扩展函数
	/// </summary>
	public static class StringExtensions {
		/// <summary>
		/// 获取最大长度的字符串
		/// 字符串长度超过最大长度时截取字符串并在后面添加后缀
		/// 后缀suffix的默认值是"..."
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="maxLength">最大长度</param>
		/// <param name="suffix">后缀，默认值是"..."</param>
		/// <returns></returns>
		public static string TruncateWithSuffix(
			this string str, int maxLength, string suffix = null) {
			if (str.Length > maxLength) {
				str = str.Substring(0, maxLength) + (suffix ?? "...");
			}
			return str;
		}
	}
}
