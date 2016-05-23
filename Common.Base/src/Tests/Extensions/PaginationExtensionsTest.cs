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
			// pageIndex = 0, pageSize = 50, linkRange = 2
			int pageIndex = 0;
			int pageSize = 50;
			var result = pagination.Paging(ref pageIndex, pageSize, query, 2, false);
			Assert.Equals(pageIndex, 0);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(0, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 2);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.Page).SequenceEqual(
				new[] { "first", "prev", "0", "1", "2", "ellipsis", "next", "last" }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "disabled", "disabled",
					"active", "enabled", "enabled", "ellipsis", "enabled", "enabled" }),
				pagination.Links);
			// pageIndex = 1, pageSize = 50; linkRange = 0
			pageIndex = 1;
			result = pagination.Paging(ref pageIndex, pageSize, query, 0, false);
			Assert.Equals(pageIndex, 1);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(50, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 1);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.Page).SequenceEqual(
				new[] { "first", "prev", "1", "next", "last" }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "active", "enabled", "enabled" }),
				pagination.Links);
			// pageIndex = 7, pageSize = 50; linkRange = 2
			pageIndex = 7;
			result = pagination.Paging(ref pageIndex, pageSize, query, 2, false);
			Assert.Equals(pageIndex, 7);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(350, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 9);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.Page).SequenceEqual(
				new[] { "first", "prev", "ellipsis", "5", "6", "7", "8", "9", "next", "last" }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "ellipsis",
					"enabled", "enabled", "active", "enabled", "enabled", "enabled", "enabled" }),
				pagination.Links);
			// pageIndex = 0x7fffffff, pageSize = 50, linkRange = 2
			pageIndex = 0x7fffffff;
			result = pagination.Paging(ref pageIndex, pageSize, query, 2, false);
			Assert.Equals(pageIndex, 9);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(450, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 9);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.IsTrueWith(pagination.Links.Select(l => l.Page).SequenceEqual(
				new[] { "first", "prev", "ellipsis", "7", "8", "9", "next", "last" }),
				pagination.Links);
			Assert.IsTrueWith(pagination.Links.Select(l => l.State).SequenceEqual(
				new[] { "enabled", "enabled", "ellipsis",
					"enabled", "enabled", "active", "disabled", "disabled" }),
				pagination.Links);
		}

		public void PagingForAjaxTableRequest() {
			var pagination = new Pagination();
			var query = Enumerable.Range(0, 500);
			// pageIndex = 0, pageSize = 50, linkRange = 1, requireTotalCount = true
			var request = new AjaxTableSearchRequest() { PageIndex = 0, PageSize = 50 };
			request.Conditions[PaginationExtensions.AjaxTablePaginationLinkRangeKey] = 1;
			request.Conditions[PaginationExtensions.AjaxTableRequireTotalCountKey] = true;
			var result = pagination.Paging(request, query);
			Assert.Equals(request.PageIndex, 0);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(0, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 1);
			Assert.IsTrue(!pagination.ReachableLastPageIsLastPage);
			Assert.Equals(pagination.TotalCount, 500);
			// pageIndex = 0x7fffffff, pageSize = 50, linkRange = 2, requireTotalCount = false
			request.PageIndex = 0x7fffffff;
			request.Conditions[PaginationExtensions.AjaxTablePaginationLinkRangeKey] = 2;
			request.Conditions[PaginationExtensions.AjaxTableRequireTotalCountKey] = false;
			result = pagination.Paging(request, query);
			Assert.Equals(request.PageIndex, 9);
			Assert.IsTrueWith(result.SequenceEqual(Enumerable.Range(450, 50)), result);
			Assert.Equals(pagination.ReachableLastPage, 9);
			Assert.IsTrue(pagination.ReachableLastPageIsLastPage);
			Assert.Equals(pagination.TotalCount, null);
		}
	}
}
