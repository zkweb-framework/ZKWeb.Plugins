using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品销售信息的显示字段
	/// </summary>
	public interface IProductSalesInfoDisplayField {
		/// <summary>
		/// 字段名称
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 获取显示的Html
		/// 不显示时可返回空或null
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="product">商品</param>
		/// <returns></returns>
		string GetDisplayHtml(DatabaseContext context, Database.Product product);
	}
}
