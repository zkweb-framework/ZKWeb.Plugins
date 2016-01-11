using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 后台应用构建器的接口
	/// 用于绕开泛型获取
	/// </summary>
	public interface IAdminAppBuilder {
		/// <summary>
		/// 应用名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// url链接
		/// </summary>
		string Url { get; }
		/// <summary>
		/// 获取搜索结果的Url
		/// </summary>
		string SearchUrl { get; }
		/// <summary>
		/// 添加数据的Url
		/// </summary>
		string AddUrl { get; }
		/// <summary>
		/// 编辑数据的Url
		/// </summary>
		string EditUrl { get; }
		/// <summary>
		/// 批量操作的Url
		/// </summary>
		string BatchUrl { get; }
		/// <summary>
		/// 获取管理的数据类型
		/// </summary>
		/// <returns></returns>
		Type GetDataType();
		/// <summary>
		/// 网站启动时的处理
		/// </summary>
		void OnWebsiteStart();
	}
}
