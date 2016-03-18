using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 数据仓储的接口
	/// </summary>
	public interface IRepository {
		/// <summary>
		/// 当前使用的数据库上下文
		/// </summary>
		DatabaseContext Context { get; set; }
	}
}
