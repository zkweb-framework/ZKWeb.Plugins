using System;
using ZKWeb.Plugins.Common.Admin.src.Domain.Filters;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters {
	/// <summary>
	/// 自动检查和设置实体的所属用户
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class CheckOwnerAttribute : ActionFilterAttribute {
		/// <summary>
		/// 启用所属用户使用的查询和操作过滤器 
		/// </summary>
		public override Func<IActionResult> Filter(Func<IActionResult> action) {
			return () => {
				var uow = Application.Ioc.Resolve<IUnitOfWork>();
				var filter = new OwnerFilter();
				using (uow.Scope())
				using (uow.EnableQueryFilter(filter))
				using (uow.EnableOperationFilter(filter)) {
					return action();
				}
			};
		}
	}
}
