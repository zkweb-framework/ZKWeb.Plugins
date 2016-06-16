using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// Ajax表格回调的扩展函数
	/// </summary>
	public static class IAjaxTableCallbackExtensions {
		/// <summary>
		/// 根据指定的表格回调获取它的所有扩展回调
		/// 并合并到一个列表返回，列表包含 [ 原始回调, 扩展回调... ]
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="callback">原始的表格回调</param>
		/// <returns></returns>
		public static IList<IAjaxTableCallback<TData>> WithExtensions<TData>(
			this IAjaxTableCallback<TData> callback) {
			var result = new List<IAjaxTableCallback<TData>>() { callback };
			var extensionType = typeof(IAjaxTableCallbackExtension<,>)
				.MakeGenericType(typeof(TData), callback.GetType());
			result.AddRange(Application.Ioc.ResolveMany(extensionType).OfType<IAjaxTableCallback<TData>>());
			return result;
		}
	}
}
