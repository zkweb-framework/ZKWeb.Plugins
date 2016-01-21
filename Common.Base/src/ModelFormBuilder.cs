using DotLiquid;
using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 从模型构建表单的构建器
	/// 例子
	/// public class TestForm : ModelFormBuilder {
	///		[Required]
	///		[TextBoxField("FieldA", "Please enter something")]
	///		public string FieldA { get; set; }
	///		[TextBoxField("FieldB", "Please enter something")]
	///		public string FieldB { get; set; }
	///		protected override void OnBind() {
	///			FieldA = "Default value";
	///		}
	///		protected override object OnSubmit() {
	///			return new { message = string.Format("{0}, {1}", FieldA, FieldB) };
	///		}
	/// }
	/// </summary>
	public abstract class ModelFormBuilder : IModelFormBuilder, ILiquidizable {
		/// <summary>
		/// 表单构建器
		/// </summary>
		public FormBuilder Form { get; protected set; }
		/// <summary>
		/// 表单字段到成员信息
		/// </summary>
		public Dictionary<FormField, PropertyInfo> FieldToProperty { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ModelFormBuilder(FormBuilder form = null) {
			Form = form ?? Application.Ioc.Resolve<FormBuilder>();
			var type = GetType();
			// 设置表单属性
			var formAttribute = type.GetAttribute<FormAttribute>();
			if (formAttribute != null) {
				Form.Attribute = formAttribute;
			}
			// 添加成员和验证属性
			Form.Fields.Clear();
			FieldToProperty = new Dictionary<FormField, PropertyInfo>();
			foreach (var member in type.GetProperties()) {
				var fieldAttribute = member.GetAttribute<FormFieldAttribute>();
				if (fieldAttribute != null) {
					var field = new FormField(fieldAttribute);
					field.ValidationAttributes.AddRange(member.GetAttributes<ValidationAttribute>());
					Form.Fields.Add(field);
					FieldToProperty[field] = member;
				}
			}
		}

		/// <summary>
		/// 绑定时的处理
		/// </summary>
		protected abstract void OnBind();

		/// <summary>
		/// 提交时的处理，返回处理结果
		/// </summary>
		protected abstract object OnSubmit();

		/// <summary>
		/// 绑定表单
		/// </summary>
		public void Bind() {
			// 绑定值到模型
			OnBind();
			// 把模型中的值设置到字段
			foreach (var field in Form.Fields) {
				var property = FieldToProperty.GetOrDefault(field);
				if (property != null) {
					field.Value = property.GetValue(this);
				}
			}
		}

		/// <summary>
		/// 提交表单，返回处理结果
		/// </summary>
		/// <returns></returns>
		public object Submit() {
			// 把提交的值设置到模型
			var submitValues = HttpContext.Current.Request.GetParams();
			var values = Form.ParseValues(submitValues);
			foreach (var field in Form.Fields) {
				var value = values.GetOrDefault(field.Attribute.Name);
				var property = FieldToProperty.GetOrDefault(field);
				if (property != null) {
					property.SetValue(this, value.ConvertOrDefault(property.PropertyType, null));
				}
			}
			// 调用提交时的处理
			return OnSubmit();
		}

		/// <summary>
		/// 获取表单属性
		/// </summary>
		/// <returns></returns>
		FormAttribute IModelFormBuilder.GetFormAttribute() {
			return Form.Attribute;
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return ((ILiquidizable)Form).ToLiquid();
		}

		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Form.ToString();
		}
	}
}
