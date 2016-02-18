using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 用于编辑指定数据的表单的回调
	/// 使用这个回调可以扩展原有的表单
	/// 例子
	/// [ExportMany]
	/// public class EditFormCallback :
	///		IDataEditFormCallback[Product, ProductEditFormForSeller] {
	///		// 实现函数
	///	}
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">指定表单的类型</typeparam>
	public interface IDataEditFormCallback<TData, TForm>
		where TData : class, new() {
		/// <summary>
		/// 表单创建时的处理
		/// </summary>
		/// <param name="form">表单</param>
		void OnCreated(TForm form);

		/// <summary>
		/// 绑定数据到表单的处理，这个函数会在原表单绑定后调用
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="bindFrom">来源的数据</param>
		void OnBind(TForm form, DatabaseContext context, TData bindFrom);

		/// <summary>
		/// 保存表单到数据，这个函数会在原表单保存后调用
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="saveTo">保存到的数据</param>
		void OnSubmit(TForm form, DatabaseContext context, TData saveTo);

		/// <summary>
		/// 数据保存后的处理，用于添加关联数据等
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="saved">已保存的数据，Id已分配</param>
		void OnSubmitSaved(TForm form, DatabaseContext context, TData saved);
	}
}
