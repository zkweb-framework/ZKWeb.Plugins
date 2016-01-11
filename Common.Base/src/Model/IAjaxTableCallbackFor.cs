using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 注册关联指定类型的表格回调
	/// 关联类型和数据类型不一定有关系，关联类型起的是标签的作用
	/// 例子
	/// [ExportMany]
	/// public class TableCallback :
	///		IAjaxTableCallbackFor[Order, OrderListForSeller] {
	///		// 实现函数
	/// }
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	/// <typeparam name="TReleated">关联类型</typeparam>
	public interface IAjaxTableCallbackFor<TData, TReleated> : IAjaxTableCallback<TData> { }
}
