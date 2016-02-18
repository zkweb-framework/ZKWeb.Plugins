using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;

namespace ZKWeb.Plugins.Common.GenericTag.src.Repositories {
	/// <summary>
	/// 通用分类的数据仓储
	/// </summary>
	[ExportMany]
	public class GenericTagRepository : GenericRepository<Database.GenericTag> {
		/// <summary>
		/// 检查指定的标签列表是否都属于指定的类型
		/// 如果标签不存在会被跳过
		/// 常用于防止越权操作
		/// </summary>
		/// <param name="idList">标签的Id列表</param>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public bool IsAllTagsTypeEqualTo(IList<object> idList, string type) {
			return Count(t => idList.Contains(t.Id) && t.Type != type) == 0;
		}
	}
}
