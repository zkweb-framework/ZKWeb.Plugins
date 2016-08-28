using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions {
	/// <summary>
	/// 静态表格数据的搜索回应的扩展函数
	/// </summary>
	public static class StaticTableSearchResponseExtensions {
		/// <summary>
		/// 转换搜索回应到静态表格构建器
		/// 请手动添加列到返回结果，添加后可以直接描画到模板
		/// </summary>
		/// <param name="response">搜索回应</param>
		/// <returns></returns>
		public static StaticTableBuilder ToTableBuilder(this StaticTableSearchResponse response) {
			var builder = new StaticTableBuilder();
			builder.Rows.AddRange(response.Rows);
			return builder;
		}
	}
}
