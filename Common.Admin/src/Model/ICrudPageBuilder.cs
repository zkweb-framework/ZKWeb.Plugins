using System;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 增删查改页面的构建器接口
	/// </summary>
	public interface ICrudPageBuilder {
		/// <summary>
		/// 名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// 基础链接
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
		/// 返回空值时表示不支持编辑数据
		/// </summary>
		string EditUrl { get; }
		/// <summary>
		/// 批量操作的Url
		/// 返回空值时表示不支持批量操作数据
		/// </summary>
		string BatchUrl { get; }
		/// <summary>
		/// 允许操作的用户类型
		/// </summary>
		UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 查看需要的权限
		/// </summary>
		string[] ViewPrivileges { get; }
		/// <summary>
		/// 编辑需要的权限
		/// </summary>
		string[] EditPrivileges { get; }
		/// <summary>
		/// 删除需要的权限
		/// </summary>
		string[] DeletePrivileges { get; }
		/// <summary>
		/// 永久删除需要的权限
		/// </summary>
		string[] DeleteForeverPrivilege { get; }
		/// <summary>
		/// 数据类型
		/// </summary>
		Type DataType { get; }
		/// <summary>
		/// 数据类型的名称
		/// </summary>
		string DataTypeName { get; }
		/// <summary>
		/// 列表页中使用的表格Id
		/// </summary>
		string ListTableId { get; }
		/// <summary>
		/// 列表页的模板路径
		/// </summary>
		string ListTemplatePath { get; }
		/// <summary>
		/// 添加页的模板路径
		/// </summary>
		string AddTemplatePath { get; }
		/// <summary>
		/// 编辑页的模板路径
		/// </summary>
		string EditTemplatePath { get; }
	}
}
