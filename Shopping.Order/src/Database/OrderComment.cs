using FluentNHibernate.Mapping;
using System;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 订单留言
	/// </summary>
	public class OrderComment {
		/// <summary>
		/// 留言Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 订单
		/// </summary>
		public virtual Order Order { get; set; }
		/// <summary>
		/// 留言人
		/// 不注册下单时的留言人等于null
		/// </summary>
		public virtual User Creator { get; set; }
		/// <summary>
		/// 买家或卖家留言
		/// </summary>
		public virtual OrderCommentSide Side { get; set; }
		/// <summary>
		/// 留言内容
		/// </summary>
		public virtual string Content { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
	}

	/// <summary>
	/// 订单留言的数据库结构
	/// </summary>
	[ExportMany]
	public class OrderCommentMap : ClassMap<OrderComment> {
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderCommentMap() {
			Id(c => c.Id);
			References(c => c.Order).Not.Nullable();
			References(c => c.Creator);
			Map(c => c.Side);
			Map(c => c.Content).Length(0xffff);
			Map(c => c.CreateTime);
		}
	}
}
