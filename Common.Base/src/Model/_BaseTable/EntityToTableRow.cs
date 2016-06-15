using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 转换实体对象到表格行时使用的类
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	public class EntityToTableRow<TEntity> {
		/// <summary>
		/// 实体对象
		/// </summary>
		public TEntity Entity { get; set; }
		/// <summary>
		/// 表格行
		/// </summary>
		public IDictionary<string, object> Row { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="entity"></param>
		public EntityToTableRow(TEntity entity) {
			Entity = entity;
			Row = new Dictionary<string, object>();
		}
	}
}
