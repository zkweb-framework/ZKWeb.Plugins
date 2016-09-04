using System;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders.Bases {
	/// <summary>
	/// 商品匹配数据的条件绑定器
	/// 绑定器只在商品编辑时使用
	/// </summary>
	public abstract class ProductMatchedDataConditionBinder {
		/// <summary>
		/// 用于编辑条件的Html
		/// </summary>
		public string Contents { get; set; }
		/// <summary>
		/// 绑定数据到Html的函数
		/// 例 function($binder, conditions) { $binder.find("input").val(conditions.Example) }
		/// </summary>
		public string Bind { get; set; }
		/// <summary>
		/// 从Html收集数据的函数
		/// 例 function($binder, conditions) { conditions.Example = $binder.find("input").val(); }
		/// </summary>
		public string Collect { get; set; }
		/// <summary>
		/// 转换条件到显示字符串的函数
		/// 例 function(conditions) { return "Example: " + conditions.Example; }
		/// </summary>
		public string Display { get; set; }

		/// <summary>
		/// 初始化绑定器，返回是否成功
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <returns></returns>
		public abstract bool Init(Guid? categoryId);
	}
}
