using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions {
	/// <summary>
	/// 静态表格处理器的扩展函数
	/// </summary>
	public static class IStaticTableHandlerExtensions {
		/// <summary>
		/// 根据指定的表格处理器获取它的所有扩展处理器
		/// 并合并到一个列表返回，列表包含 [ 原始处理器, 扩展处理器... ]
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="callback">原始的表格处理器</param>
		/// <returns></returns>
		public static IList<IStaticTableHandler<TEntity, TPrimaryKey>>
			WithExtraHandlers<TEntity, TPrimaryKey>(
			this IStaticTableHandler<TEntity, TPrimaryKey> handler)
			where TEntity : class, IEntity<TPrimaryKey> {
			var result = new List<IStaticTableHandler<TEntity, TPrimaryKey>>() { handler };
			var extensionType = typeof(IStaticTableExtraHandler<,,>)
				.MakeGenericType(typeof(TEntity), typeof(TPrimaryKey), handler.GetType());
			result.AddRange(Application.Ioc.ResolveMany(extensionType)
				.OfType<IStaticTableHandler<TEntity, TPrimaryKey>>());
			return result;
		}
	}
}
