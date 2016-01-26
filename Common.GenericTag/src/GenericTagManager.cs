using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.GenericTag.src {
	/// <summary>
	/// 通用分类管理器
	/// </summary>
	[ExportMany]
	public class GenericTagManager {
		/// <summary>
		/// 检查指定的标签列表是否都属于指定的类型
		/// 如果标签不存在会被跳过
		/// 常用于防止越权操作
		/// </summary>
		/// <param name="idList">标签的Id列表</param>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public bool IsAllTagsTypeEqualTo(IList<long> idList, string type) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				return context.Count<Database.GenericTag>(t => idList.Contains(t.Id) && t.Type != type) == 0;
			}
		}
	}
}
