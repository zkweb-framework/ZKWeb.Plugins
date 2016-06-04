using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// 数据查询对象的扩展函数
	/// </summary>
	public static class IQueryableExtensions {
		/// <summary>
		/// 按回收站状态过滤数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="query">查询对象</param>
		/// <param name="request">搜索请求</param>
		public static IQueryable<TData> FilterByRecycleBin<TData>(
			this IQueryable<TData> query, AjaxTableSearchRequest request) {
			var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
			var propertyName = RecyclableTrait.For<TData>().PropertyName;
			return query.Where(
				ExpressionUtils.MakeMemberEqualiventExpression<TData>(propertyName, deleted));
		}
	}
}
