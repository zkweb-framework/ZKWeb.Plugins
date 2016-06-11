using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;

namespace ZKWeb.Plugins.Common.Admin.src.Scaffolding {
	/// <summary>
	/// 用于编辑指定数据的表单，支持多标签
	/// 绑定和提交表单时会进行数据所有权的检查，提交时注意调用AssignOwnedUser
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class TabUserOwnedDataEditFormBuilder<TData, TForm> :
		UserOwnedDataEditFormBuilder<TData, TForm>
		where TData : class, new() {
		/// <summary>
		/// 初始化
		/// </summary>
		public TabUserOwnedDataEditFormBuilder() : base(new TabFormBuilder()) { }
	}
}
