using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.UnitTest;

namespace ZKWeb.Plugins.Common.Base.src.Tests.Extensions {
	[UnitTest]
	public class PaginationExtensionsTest {
		public void Paging() {
			var pagination = new Pagination();
			var query = Enumerable.Range(0, 500);

			// pageNo = 1, pageSize = 50, linkRange = 2
			int pageNo = 1;
			int pageSize = 50;
			var result = pagination.Paging(ref pageNo, pageSize, query, 2, false);
			Assert.Equals(pageNo, 1);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(0, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 3);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.PageNo).SequenceEqual(
				new[] { 1, 1, 1, 2, 3, int.MaxValue, 1, int.MaxValue }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "disabled", "disabled",
					"active", "enabled", "enabled", "ellipsis", "enabled", "enabled" }),
				pagination.Links);

			// pageNo = 2, pageSize = 50; linkRange = 0
			pageNo = 2;
			result = pagination.Paging(ref pageNo, pageSize, query, 0, false);
			Assert.Equals(pageNo, 2);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(50, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 2);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.PageNo).SequenceEqual(
				new[] { 1, 1, 2, 3, int.MaxValue }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "active", "enabled", "enabled" }),
				pagination.Links);

			// pageNo = 8, pageSize = 50; linkRange = 2
			pageNo = 8;
			result = pagination.Paging(ref pageNo, pageSize, query, 2, false);
			Assert.Equals(pageNo, 8);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(350, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 10);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.PageNo).SequenceEqual(
				new[] { 1, 7, 1, 6, 7, 8, 9, 10, 9, int.MaxValue }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "ellipsis",
					"enabled", "enabled", "active", "enabled", "enabled", "enabled", "enabled" }),
				pagination.Links);

			// pageNo = 0x7fffffff, pageSize = 50, linkRange = 2
			pageNo = 0x7fffffff;
			result = pagination.Paging(ref pageNo, pageSize, query, 2, false);
			Assert.Equals(pageNo, 10);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(450, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 10);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.PageNo).SequenceEqual(
				new[] { 1, 9, 1, 8, 9, 10, 10, int.MaxValue }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "ellipsis",
					"enabled", "enabled", "active", "disabled", "disabled" }),
				pagination.Links);
		}

		public void PagingForAjaxTableRequest() {
			var pagination = new Pagination();
			var query = Enumerable.Range(0, 500);

			// pageNo = 1, pageSize = 50, linkRange = 1, requireTotalCount = true
			var request = new AjaxTableSearchRequest() { PageNo = 1, PageSize = 50 };
			request.Conditions[PaginationExtensions.AjaxTablePaginationLinkRangeKey] = 1;
			request.Conditions[PaginationExtensions.AjaxTableRequireTotalCountKey] = true;
			var result = pagination.Paging(request, query);
			Assert.Equals(request.PageNo, 0);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(0, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 2);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.Equals(pagination.TotalCount, 500);

			// pageNo = 0x7fffffff, pageSize = 50, linkRange = 2, requireTotalCount = false
			request.PageNo = 0x7fffffff;
			request.Conditions[PaginationExtensions.AjaxTablePaginationLinkRangeKey] = 2;
			request.Conditions[PaginationExtensions.AjaxTableRequireTotalCountKey] = false;
			result = pagination.Paging(request, query);
			Assert.Equals(request.PageNo, 10);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(450, 50)), result);
			Assert.Equals(pagination.ReachableLastPageNo, 10);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.Equals(pagination.TotalCount, null);
		}
	}
}
