using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格数据的搜索请求的扩展函数
	/// </summary>
	public static class AjaxTableSearchRequestExtensions {
		/// <summary>
		/// 从数据库中的数据构建搜索回应
		/// 支持自动分页和配合表格处理器设置结果
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="request">搜索请求</param>
		/// <param name="handlers">表格处理器</param>
		/// <returns></returns>
		public static AjaxTableSearchResponse BuildResponseFromDatabase<TEntity, TPrimaryKey>(
			this AjaxTableSearchRequest request,
			IEnumerable<IAjaxTableHandler<TEntity, TPrimaryKey>> handlers)
			where TEntity : class, IEntity<TPrimaryKey> {
			var response = new AjaxTableSearchResponse();
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			var result = service.GetMany(query => {
				// 从数据库获取数据，过滤并排序
				foreach (var handler in handlers) {
					handler.OnQuery(request, ref query);
				}
				foreach (var handler in handlers) {
					handler.OnSort(request, ref query);
				}
				// 分页并设置分页信息
				// 当前页没有任何内容时返回最后一页的数据
				return response.Pagination.Paging(request, query);
			});
			response.PageNo = request.PageNo;
			response.PageSize = request.PageSize;
			// 选择数据
			// 默认把对象转换到的字符串保存到ToString中
			var pairs = result.Select(r => new EntityToTableRow<TEntity>(r)).ToList();
			foreach (var pair in pairs) {
				pair.Row["ToString"] = pair.Entity.ToString();
			}
			foreach (var callback in handlers) {
				callback.OnSelect(request, pairs);
			}
			response.Rows = pairs.Select(p => p.Row).ToList();
			// 调用返回搜索回应前的处理器，这里会添加需要的列
			foreach (var handler in handlers) {
				handler.OnResponse(request, response);
			}
			return response;
		}
	}
}
