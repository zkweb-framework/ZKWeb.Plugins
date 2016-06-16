using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 根据枚举值提供选项列表
	/// </summary>
	/// <typeparam name="TEnum">枚举类型</typeparam>
	public class ListItemFromEnum<TEnum> : IListItemProvider where TEnum : struct {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (Enum value in Enum.GetValues(typeof(TEnum))) {
				yield return new ListItem(new T(value.GetDescription()), ((int)(object)value).ToString());
			}
		}
	}
}
