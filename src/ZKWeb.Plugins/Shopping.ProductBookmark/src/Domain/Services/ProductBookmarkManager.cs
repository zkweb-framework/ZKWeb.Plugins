using System;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Domain.Services {
	using Product = Product.src.Domain.Entities.Product;
	using ProductBookmark = Entities.ProductBookmark;

	/// <summary>
	/// 商品收藏的管理器
	/// </summary>
	[ExportMany]
	public class ProductBookmarkManager : DomainServiceBase<ProductBookmark, Guid> {
		/// <summary>
		/// 添加收藏的商品
		/// 添加成功返回true，已经存在返回false
		/// 商品不存在或已删除等时抛出例外
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="productId">商品Id</param>
		/// <returns></returns>
		public virtual bool Add(Guid userId, Guid productId) {
			using (UnitOfWork.Scope()) {
				UnitOfWork.Context.BeginTransaction();
				if (IsBookmarked(userId, productId))
					return false;
				var userRepository = Application.Ioc.Resolve<IRepository<User, Guid>>();
				var productRepository = Application.Ioc.Resolve<IRepository<Product, Guid>>();
				var bookmark = new ProductBookmark() {
					Owner = userRepository.Get(u => u.Id == userId),
					Product = productRepository.Get(p => p.Id == productId)
				};
				Save(ref bookmark);
				UnitOfWork.Context.FinishTransaction();
				return true;
			}
		}

		/// <summary>
		/// 判断商品是否已收藏
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="productId">商品Id</param>
		/// <returns></returns>
		public virtual bool IsBookmarked(Guid userId, Guid productId) {
			using (UnitOfWork.Scope()) {
				return Repository.Query().Any(b => b.Owner.Id == userId && b.Product.Id == productId);
			}
		}
	}
}
