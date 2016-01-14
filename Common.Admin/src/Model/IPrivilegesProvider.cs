﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 权限提供器
	/// 推荐使用的权限字符串的格式
	///		Group:Name
	/// 例如
	///		Admin Manage:View
	///		Admin Manage:Edit
	///		Admin Manage:Delete
	///		Admin Manage:DeleteForever
	/// 不使用此格式时权限会被归到"其他"分组下
	/// </summary>
	public interface IPrivilegesProvider {
		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetPrivileges();
	}
}