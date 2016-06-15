using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 网站附加配置中使用的键
	/// </summary>
	internal class ExtraConfigKeys {
		/// <summary>
		/// 购物车商品的总数量的缓存时间，单位是秒
		/// </summary>
		public const string CartProductTotalCountCacheTime = "Shopping.Order.CartProductTotalCountCacheTime";
		/// <summary>
		/// 非会员添加购物车商品时，保留会话的天数，单位是天
		/// </summary>
		public const string SessionExpireDaysForNonUserPurcharse =
			"Shopping.Order.SessionExpireDaysForNonUserPurcharse";
	}
}
