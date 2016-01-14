using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 权限名称翻译器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PrivilegesTranslator {
		/// <summary>
		/// 获取权限的本地化名称
		/// </summary>
		/// <param name="privilege">权限</param>
		/// <returns></returns>
		public virtual string Translate(string privilege) {
			var index = privilege.IndexOf(':');
			var group = index > 0 ? privilege.Substring(0, index) : "Other";
			var name = index > 0 ? privilege.Substring(index + 1) : privilege;
			return string.Format("{0}:{1}", new T(group), new T(name));
		}
	}
}
