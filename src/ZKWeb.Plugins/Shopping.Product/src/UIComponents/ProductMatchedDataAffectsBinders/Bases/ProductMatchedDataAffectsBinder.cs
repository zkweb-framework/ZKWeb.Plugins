using System;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases {
	/// <summary>
	/// 商品匹配数据的影响数据绑定器
	/// 绑定器只在商品编辑时使用
	/// </summary>
	public abstract class ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 列头部显示的Html
		/// </summary>
		public string Header { get; set; }
		/// <summary>
		/// 用于编辑影响数据的Html
		/// </summary>
		public string Contents { get; set; }
		/// <summary>
		/// 绑定数据到Html的函数
		/// 例 function($binder, affects) { $binder.find("input").val(affects.Example) }
		/// </summary>
		public string Bind { get; set; }
		/// <summary>
		/// 从Html收集数据的函数
		/// 例 function($binder, affects) { affects.Example = $binder.find("input").val(); }
		/// </summary>
		public string Collect { get; set; }
		/// <summary>
		/// 显示顺序
		/// </summary>
		public int DisplayOrder { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMatchedDataAffectsBinder() {
			DisplayOrder = int.MaxValue;
		}

		/// <summary>
		/// 初始化绑定器，返回是否成功
		/// </summary>
		/// <param name="categoryId">类目Id</param>
		/// <returns></returns>
		public abstract bool Init(Guid? categoryId);
	}
}
