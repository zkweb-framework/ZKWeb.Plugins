using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 表单构建器
	/// </summary>
	public class FormBuilder {
		

		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return base.ToString();
		}

		/// <summary>
		/// 表单字段
		/// </summary>
		public class FormField {
			/// <summary>
			/// 表单字段的属性
			/// </summary>
			public FormFieldAttribute Attribute { get; set; }
			/// <summary>
			/// 字段的值
			/// </summary>
			public object Value { get; set; }
		}
	}
}
