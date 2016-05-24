using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// Id列（多选框+批量操作菜单）
	/// </summary>
	public class AjaxTableIdColumn : AjaxTableColumn {
		/// <summary>
		/// 菜单项的模板列表，ul中的li
		/// </summary>
		public virtual List<MenuItem> ActionTemplates { get; set; }
		/// <summary>
		/// 头部模板字符串
		/// </summary>
		public override string HeadTemplate
		{
			get
			{
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				var html = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.id_column_head.html",
					new { actionTemplates = ActionTemplates });
				return html;
			}
			set { }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableIdColumn() {
			ActionTemplates = new List<MenuItem>();
		}
	}
}
