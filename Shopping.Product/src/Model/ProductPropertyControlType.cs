using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品属性使用的输入控件类型
	/// </summary>
	public enum ProductPropertyControlType {
		/// <summary>
		/// 文本框
		/// </summary>
		TextBox = 0,
		/// <summary>
		/// 多选框
		/// </summary>
		CheckBox = 1,
		/// <summary>
		/// 单选框
		/// </summary>
		RadioButton = 2,
		/// <summary>
		/// 下拉列表
		/// </summary>
		DropdownList = 3,
		/// <summary>
		/// 可编辑的下拉列表
		/// </summary>
		EditableDropdownList = 4
	}
}
