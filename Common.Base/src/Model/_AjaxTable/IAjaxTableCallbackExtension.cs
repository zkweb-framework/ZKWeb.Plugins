using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 扩展指定类型的表格回调使用的接口
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	/// <typeparam name="TCallback">关联到的表格回调类型</typeparam>
	public interface IAjaxTableCallbackExtension<TData, TCallback> :
		IAjaxTableCallback<TData>
		where TCallback : IAjaxTableCallback<TData> { }
}
