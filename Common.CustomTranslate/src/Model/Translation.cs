using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Model {
	/// <summary>
	/// 翻译内容
	/// </summary>
	public class Translation {
		/// <summary>
		/// 原文
		/// </summary>
		public string Original { get; set; }
		/// <summary>
		/// 译文
		/// </summary>
		public string Translated { get; set; }

		/// <summary>
		/// 显示原文
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Original;
		}
	}
}
