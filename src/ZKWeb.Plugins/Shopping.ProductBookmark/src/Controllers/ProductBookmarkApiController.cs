using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Shopping.ProductBookmark.src.Domain.Services;
using ZKWeb.Web;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Controllers {
	/// <summary>
	/// 商品收藏的Api控制器
	/// </summary>
	[ExportMany]
	public class ProductBookmarkApiController : ControllerBase {
		/// <summary>
		/// 添加收藏的商品
		/// </summary>
		[Action("api/product_bookmarks/add", HttpMethods.POST)]
		public object Add(Guid productId) {
			// 获取当前登录的用户
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = session.GetUser();
			if (user == null) {
				// 用户不存在时跳转到登录页面
				return new {
					message = new T("Bookmark product require login, redirecting to login page..."),
					script = BaseScriptStrings.Redirect("/user/login", 1500)
				};
			}
			// 调用管理器添加
			var productBookmarkManager = Application.Ioc.Resolve<ProductBookmarkManager>();
			if (!productBookmarkManager.Add(user.Id, productId)) {
				return new { message = new T("Product already bookmarked"), bookmarked = true };
			}
			return new { message = new T("Bookmark product success"), bookmarked = true };
		}

		/// <summary>
		/// 判断商品是否已收藏
		/// </summary>
		[Action("api/product_bookmarks/is_bookmarked", HttpMethods.POST)]
		public object IsBookmarked(Guid productId) {
			// 获取当前登录的用户
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = session.GetUser();
			if (user == null) {
				return new { bookmarked = false };
			}
			// 调用管理器判断
			var productBookmarkManager = Application.Ioc.Resolve<ProductBookmarkManager>();
			var bookmarked = productBookmarkManager.IsBookmarked(user.Id, productId);
			return new { bookmarked };
		}
	}
}
