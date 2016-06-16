using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单字段
	/// </summary>
	public class FormField : ILiquidizable {
		/// <summary>
		/// 表单字段的属性
		/// </summary>
		public FormFieldAttribute Attribute { get; set; }
		/// <summary>
		/// 验证表单的属性列表
		/// </summary>
		public List<Attribute> ValidationAttributes { get; set; }
		/// <summary>
		/// 字段的值
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="attribute">表单字段的属性</param>
		public FormField(FormFieldAttribute attribute) {
			Attribute = attribute;
			ValidationAttributes = new List<Attribute>();
			Value = null;
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(ToString());
		}

		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			// 根据验证器添加html属性
			var htmlAttributes = new Dictionary<string, string>();
			if (ValidationAttributes.Count > 0) {
				htmlAttributes["data-val"] = "true"; // jquery.validate.unobtrusive使用的属性
			}
			foreach (var validatorAttribute in ValidationAttributes) {
				var validator = Application.Ioc.Resolve<IFormFieldValidator>(serviceKey: validatorAttribute.GetType());
				validator.AddHtmlAttributes(this, validatorAttribute, htmlAttributes);
			}
			// 查找处理器并构建表单字段的html
			var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: Attribute.GetType());
			return fieldHandler.Build(this, htmlAttributes);
		}
	}
}
