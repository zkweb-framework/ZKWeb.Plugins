﻿using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.FieldFactories {
	/// <summary>
	/// 单选按钮组生成器
	/// </summary>
	[ExportMany(ContractKey = "RadioButtons")]
	public class RadioButtonsFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var source = fieldData.GetOrDefault<string>("Source");
			var group = fieldData.GetOrDefault<string>("Group");
			var provider = Application.Ioc.Resolve<IListItemProvider>(IfUnresolved.ReturnDefault, source);
			var sourceType = provider?.GetType();
			return new RadioButtonsFieldAttribute(name, sourceType) { Group = group };
		}
	}
}
