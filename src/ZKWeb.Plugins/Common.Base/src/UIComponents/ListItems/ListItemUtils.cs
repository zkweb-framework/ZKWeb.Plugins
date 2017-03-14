using System;
using ZKWebStandard.Collections;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 选项工具类
	/// </summary>
	public static class ListItemUtils {
		/// <summary>
		/// 获取选项值和选项列表提供器
		/// </summary>
		/// <typeparam name="T">提供器的类型</typeparam>
		/// <param name="source">手动指定的提供器类型, 如果等于null则要求value必须是ListItemValueWithProvider</param>
		/// <param name="value">值, 如果source等于null应该使用ListItemValueWithProvider</param>
		/// <returns></returns>
		public static Pair<object, T> GetValueAndProvider<T>(Type source, object value) {
			if (source == null) {
				var valueWithProvider = value as ListItemValueWithProvider;
				if (valueWithProvider == null) {
					throw new ArgumentException(
						"Type of list item provider is null, please use ListItemValueWithProvider as value type");
				}
				return Pair.Create(valueWithProvider.Value, (T)valueWithProvider.Provider);
			}
			return Pair.Create(value, (T)Activator.CreateInstance(source));
		}

		/// <summary>
		/// 解析值时如果source等于null则用ListItemValueWithProvider包装
		/// 否则返回原值
		/// </summary>
		/// <param name="source">手动指定的提供器类型</param>
		/// <param name="parsed">解析后的值</param>
		/// <returns></returns>
		public static object WrapValueAndProvider(Type source, object parsed) {
			if (source == null) {
				return new ListItemValueWithProvider(parsed, null);
			}
			return parsed;
		}
	}
}
