using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm {
	/// <summary>
	/// 动态表单构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// 构建后的表单不带值，需要手动绑定
	/// </summary>
	/// <example>
	/// 字段数据示例
	/// {
	///		"Type": "TextBox",
	///		"Name": "ExampleField",
	///		"Validators": [
	///			{ "Type": "Required" }
	///		]
	/// }
	/// </example>
	[ExportMany]
	public class DynamicFormBuilder {
		/// <summary>
		/// 字段数据列表
		/// </summary>
		protected IList<IDictionary<string, object>> FieldDatas { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DynamicFormBuilder() {
			FieldDatas = new List<IDictionary<string, object>>();
		}

		/// <summary>
		/// 根据数据添加字段
		/// </summary>
		/// <param name="fieldData">字段数据</param>
		public virtual void AddField(IDictionary<string, object> fieldData) {
			FieldDatas.Add(fieldData);
		}

		/// <summary>
		/// 根据数据列表添加字段列表
		/// </summary>
		/// <param name="fieldDatas">字段数据列表</param>
		public virtual void AddFields(IEnumerable<IDictionary<string, object>> fieldDatas) {
			foreach (var fieldData in fieldDatas) {
				AddField(fieldData);
			}
		}

		/// <summary>
		/// 构建表单对象
		/// </summary>
		/// <typeparam name="T">表单对象类型</typeparam>
		/// <returns></returns>
		public virtual T ToForm<T>()
			where T : FormBuilder, new() {
			var result = new T();
			foreach (var fieldData in FieldDatas) {
				// 根据类型创建字段属性
				var fieldType = fieldData.GetOrDefault<string>("Type");
				var fieldFactory = Application.Ioc.Resolve<IDynamicFormFieldFactory>(serviceKey: fieldType);
				if (fieldFactory == null) {
					throw new ArgumentException($"No factory registered for dynamic form field type: '{fieldType}'");
				}
				var fieldAttribute = fieldFactory.Create(fieldData);
				var formField = new FormField(fieldAttribute);
				// 根据类型创建验证字段属性
				var validatorDatas = fieldData.GetOrDefault<IList<IDictionary<string, object>>>("Validators");
				if (validatorDatas != null) {
					foreach (var validatorData in validatorDatas) {
						var validatorType = validatorData.GetOrDefault<string>("Type");
						var validatorFactory = Application.Ioc
							.Resolve<IDynamicFormFieldValidatorFactory>(serviceKey: validatorType);
						if (validatorFactory == null) {
							throw new ArgumentException(
								$"No factory registered for dynamic form field validator type: '{validatorType}'");
						}
						var validatorAttribute = validatorFactory.Create(validatorData);
						formField.ValidationAttributes.Add(validatorAttribute);
					}
				}
				// 添加字段到表单
				result.Fields.Add(formField);
			}
			return result;
		}

		/// <summary>
		/// 构建表单对象
		/// 默认使用FormBuilder类型
		/// </summary>
		/// <returns></returns>
		public virtual FormBuilder ToForm() {
			return ToForm<FormBuilder>();
		}
	}
}
