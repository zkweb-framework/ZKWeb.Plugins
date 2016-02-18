using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Repositories;

namespace ZKWeb.Plugins.Common.GenericClass.src.Repositories {
	/// <summary>
	/// 通用分类的数据仓储
	/// </summary>
	[ExportMany]
	public class GenericClassRepository : GenericRepository<Database.GenericClass> {
		/// <summary>
		/// 检查指定的分类列表是否都属于指定的类型
		/// 如果分类不存在会被跳过
		/// 常用于防止越权操作
		/// </summary>
		/// <param name="idList">分类的Id列表</param>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public bool IsAllClassesTypeEqualTo(IList<object> idList, string type) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				return context.Count<Database.GenericClass>(t => idList.Contains(t.Id) && t.Type != type) == 0;
			}
		}
	}
}
