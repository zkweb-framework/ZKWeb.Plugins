using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.TypeTraits {
	/// <summary>
	/// 判断数据库类型是否可回收
	/// </summary>
	public class IsRecyclable<T> {
		/// <summary>
		/// 用于判断的成员名称
		/// </summary>
		public const string PropertyName = "Deleted";
		/// <summary>
		/// 判断结果
		/// </summary>
		public readonly static bool Value = typeof(T).GetProperty(PropertyName) != null;
	}
}
