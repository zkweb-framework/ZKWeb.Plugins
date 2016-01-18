using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 用于编辑指定数据的表单，支持多标签
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class TabDataEditFormBuilder<TData, TForm> :
		DataEditFormBuilder<TData, TForm> where TData : class, new() {
		/// <summary>
		/// 初始化
		/// </summary>
		public TabDataEditFormBuilder() : base(new TabFormBuilder()) { }
	}
}
