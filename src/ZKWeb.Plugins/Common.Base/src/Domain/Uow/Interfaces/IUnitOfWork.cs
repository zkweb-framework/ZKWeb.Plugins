using System;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces {
	/// <summary>
	/// 工作单元的接口
	/// </summary>
	public interface IUnitOfWork {
		/// <summary>
		/// 获取当前的数据库上下文
		/// 不存在时抛出错误
		/// </summary>
		IDatabaseContext Context { get; }

		/// <summary>
		/// 在指定的范围内使用工作单元
		/// 工作单元中的代码都使用同一个数据库上下文
		/// 这个函数可以嵌套使用，嵌套使用时都使用最上层的数据库上下文
		/// </summary>
		/// <returns></returns>
		IDisposable Scope();
	}
}