using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格处理器的扩展函数
	/// </summary>
	public static class IAjaxTableHandlerExtensions {
		/// <summary>
		/// 根据指定的表格处理器获取它的所有扩展处理器
		/// 并合并到一个列表返回，列表包含 [ 原始处理器, 扩展处理器... ]
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="handler">原始的表格处理器</param>
		/// <returns></returns>
		public static IList<IAjaxTableHandler<TEntity, TPrimaryKey>>
			WithExtraHandlers<TEntity, TPrimaryKey>(
			this IAjaxTableHandler<TEntity, TPrimaryKey> handler)
			where TEntity : class, IEntity<TPrimaryKey> {
			var result = new List<IAjaxTableHandler<TEntity, TPrimaryKey>>() { handler };
			var extensionType = typeof(IAjaxTableExtraHandler<,,>)
				.MakeGenericType(typeof(TEntity), typeof(TPrimaryKey), handler.GetType());
			result.AddRange(Application.Ioc.ResolveMany(extensionType)
				.OfType<IAjaxTableHandler<TEntity, TPrimaryKey>>());
			return result;
		}
	}
}
