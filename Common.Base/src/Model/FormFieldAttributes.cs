using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 只读文本
	/// </summary>
	public class LabelFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public LabelFieldAttribute(string name) {
			Name = name;
		}
	}

	/// <summary>
	/// 文本框
	/// </summary>
	public class TextBoxFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public TextBoxFieldAttribute(string name) {
			Name = name;
		}
	}

	/// <summary>
	/// 密码框
	/// </summary>
	public class PasswordFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public PasswordFieldAttribute(string name) {
			Name = name;
		}
	}

	/// <summary>
	/// 多行文本框
	/// </summary>
	public class TextAreaFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 行数
		/// </summary>
		public int Rows { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="rows">行数</param>
		public TextAreaFieldAttribute(string name, int rows) {
			Name = name;
			Rows = rows;
		}
	}

	/// <summary>
	/// 勾选框
	/// </summary>
	public class CheckBoxFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public CheckBoxFieldAttribute(string name) {
			Name = name;
		}
	}

	/// <summary>
	/// 下拉框
	/// </summary>
	public class DropdownListFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 选项来源，必须继承IListItemProvider
		/// </summary>
		public Type Sources { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="sources">选项来源，必须继承IListItemProvider</param>
		public DropdownListFieldAttribute(string name, Type sources) {
			Name = name;
			Sources = sources;
		}
	}

	/// <summary>
	/// 单选按钮列表
	/// </summary>
	public class RadioButtonsFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 选项来源，必须继承IListItemProvider
		/// </summary>
		public Type Sources { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="sources">选项来源，必须继承IListItemProvider</param>
		public RadioButtonsFieldAttribute(string name, Type sources) {
			Name = name;
			Sources = sources;
		}
	}

	/// <summary>
	/// 图片验证码
	/// </summary>
	public class CaptchaFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 用于区分各个验证码的键值
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="key">用于区分各个验证码的键值</param>
		public CaptchaFieldAttribute(string name, string key) {
			Name = name;
			Key = key;
		}
	}

	/// <summary>
	/// 文件上传
	/// </summary>
	public class FileUploaderFieldAttribute : FormFieldAttribute, IFormFieldRequireMultiPart {
		/// <summary>
		/// 允许的文件后缀
		/// </summary>
		public string Extensions { get; set; }
		/// <summary>
		/// 允许上传的最大长度，单位是字节
		/// </summary>
		public long MaxContentsLength { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段类型</param>
		/// <param name="extensions">允许的文件后缀，默认是图片后缀</param>
		/// <param name="maxContentsLength">允许上传的最大长度，单位是字节，默认是1MB</param>
		public FileUploaderFieldAttribute(
			string name, string extensions = "png,jpg,jpeg,gif", int maxContentsLength = 1048576) {
			Name = name;
			Extensions = extensions;
			MaxContentsLength = maxContentsLength;
		}
	}
}
