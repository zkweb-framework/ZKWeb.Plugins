using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单属性
	/// 用于指定表单名称，可省略
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class FormAttribute : Attribute {
		/// <summary>
		/// 表单名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 提交到的url
		/// </summary>
		public string Action { get; set; }
		/// <summary>
		/// 提交类型
		/// </summary>
		public string Method { get; set; }
		/// <summary>
		/// 启用Ajax提交，默认等于true
		/// </summary>
		public bool EnableAjaxSubmit { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">表单名称</param>
		/// <param name="action">提交到的url，默认是空（和请求的url相同）</param>
		/// <param name="method">提交类型，默认是POST</param>
		public FormAttribute(string name, string action = null, string method = HttpMethods.POST) {
			Name = name;
			Action = action;
			Method = method;
			EnableAjaxSubmit = true;
		}
	}
}
