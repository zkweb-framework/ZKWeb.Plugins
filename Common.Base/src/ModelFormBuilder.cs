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
	/// </summary>
	public abstract class ModelFormBuilder : ILiquidizable {
		/// <summary>
		/// 表单构建器
		/// </summary>
		protected FormBuilder Form { get; set; }
		/// <summary>
		/// 表单字段到成员信息
		/// </summary>
		protected Dictionary<FormField, MemberInfo> FieldToMember { get; set; }

		/// <summary>
		/// 初始化表单构建器
		/// </summary>
		public ModelFormBuilder(FormBuilder form = null) {
			Form = form ?? Application.Ioc.Resolve<FormBuilder>();
			var type = this.GetType();
			// 设置表单属性
			var formAttribute = type.GetAttribute<FormAttribute>();
			if (formAttribute != null) {
				Form.Attribute = formAttribute;
			}
			// 添加成员和验证属性
			Form.Fields.Clear();
			FieldToMember = new Dictionary<FormField, MemberInfo>();
			var members = type.GetProperties().OfType<MemberInfo>().Concat(type.GetFields());
			foreach (var member in members) {
				var fieldAttribute = member.GetAttribute<FormFieldAttribute>();
				if (fieldAttribute != null) {
					var field = new FormField(fieldAttribute);
					field.ValidationAttributes.AddRange(member.GetAttributes<ValidationAttribute>());
					Form.Fields.Add(field);
					FieldToMember[field] = member;
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
				var member = FieldToMember[field];
				if (member is PropertyInfo) {
					field.Value = ((PropertyInfo)member).GetValue(this);
				} else if (member is FieldInfo) {
					field.Value = ((FieldInfo)member).GetValue(this);
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
				var member = FieldToMember[field];
				if (member is PropertyInfo) {
					((PropertyInfo)member).SetValue(this, value);
				} else if (member is FieldInfo) {
					((FieldInfo)member).SetValue(this, value);
				}
			}
			// 调用提交时的处理
			return OnSubmit();
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
