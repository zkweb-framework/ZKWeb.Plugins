using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// 操作列，包含操作按钮
	/// </summary>
	public class AjaxTableActionColumn : AjaxTableColumn {
		/// <summary>
		/// 操作按钮的模板列表
		/// </summary>
		public virtual List<HtmlString> ActionTemplates { get; set; }
		/// <summary>
		/// 单元格模板字符串
		/// </summary>
		public override string CellTemplate
		{
			get { return string.Join("", ActionTemplates); }
			set { }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableActionColumn() {
			Key = "Actions";
			HeadTemplate = new T("Actions");
			ActionTemplates = new List<HtmlString>();
		}
	}
}
