﻿using DotLiquid;
using System;
using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			areaManager.GetArea("header_navbar_left").DefaultWidgets.Add("common.admin.widgets/user_login_info");
			areaManager.GetArea("header_navbar_right").DefaultWidgets.Add("common.admin.widgets/enter_admin_panel");
			areaManager.GetArea("admin_footer_area").DefaultWidgets.Add("common.base.widgets/copyright");
			areaManager.GetArea("admin_sidebar").DefaultWidgets.Add("common.admin.widgets/admin_sidebar_app_menu");
			areaManager.GetArea("admin_login_box").DefaultWidgets.Add("common.admin.widgets/admin_login_form");
			areaManager.GetArea("user_login_form_center").DefaultWidgets.Add("common.admin.widgets/user_login_form");
			areaManager.GetArea("user_reg_form_center").DefaultWidgets.Add("common.admin.widgets/user_reg_form");
			// 注册模板可描画类型
			Template.RegisterSafeType(typeof(Version), s => s.ToString());
		}
	}
}
