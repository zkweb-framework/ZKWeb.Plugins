using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm {
	/// <summary>
	/// 动态表单构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// </summary>
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
			// TODO: 动态添加字段
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
