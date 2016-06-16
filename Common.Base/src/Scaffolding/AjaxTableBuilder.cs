using DotLiquid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// Ajax表格构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// </summary>
	/// <example>
	/// var table = Application.Ioc.Resolve[AjaxTableBuilder]();
	/// table.Id = "TestList";
	/// table.Target = "/test/search";
	/// return new TemplateResult("test_table.html", new { table });
	/// </example>
	[ExportMany]
	public class AjaxTableBuilder : ILiquidizable {
		/// <summary>
		/// 表格菜单Id的后缀，表格菜单Id = 表格Id + 后缀
		/// </summary>
		public const string MenuIdSuffix = "Menu";
		/// <summary>
		/// 表格Id，必填
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 获取表格数据的url，必填
		/// </summary>
		public string Target { get; set; }
		/// <summary>
		/// 使用的模板，不设置时使用默认
		/// </summary>
		public string Template { get; set; }
		/// <summary>
		/// 右键菜单项列表，默认包含刷新和全屏
		/// </summary>
		public List<MenuItem> MenuItems { get; protected set; }
		/// <summary>
		/// 附加选项
		/// </summary>
		public Dictionary<string, object> ExtraOptions { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableBuilder() {
			MenuItems = new List<MenuItem>();
			MenuItems.AddRefresh();
			MenuItems.AddFullscreen();
			ExtraOptions = new Dictionary<string, object>();
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(ToString());
		}

		/// <summary>
		/// 获取表格html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Target)) {
				throw new ArgumentNullException("Id and Target can't be empty");
			}
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.ajax_table.html", new {
				id = Id,
				target = Target,
				template = Template,
				menuId = Id + MenuIdSuffix,
				menuItems = MenuItems,
				extraOptions = JsonConvert.SerializeObject(ExtraOptions)
			});
			return html;
		}
	}
}
