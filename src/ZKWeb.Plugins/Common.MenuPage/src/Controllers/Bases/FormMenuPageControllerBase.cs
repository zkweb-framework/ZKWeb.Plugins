using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;

namespace ZKWeb.Plugins.Common.MenuPage.src.Controllers.Bases {
	/// <summary>
	/// 带单个表单的菜单页面控制器的基础类
	/// 需要再次经过包装，请勿直接使用
	/// </summary>
	public abstract class FormMenuPageControllerBase : SimpleMenuPageControllerBase {
		/// <summary>
		/// 模板路径
		/// </summary>
		public abstract string TemplatePath { get; }
		/// <summary>
		/// 获取表单构建器
		/// </summary>
		/// <returns></returns>
		protected abstract IModelFormBuilder GetForm();

		/// <summary>
		/// 请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			var form = GetForm();
			if (Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult(TemplatePath, new { title = Name, iconClass = IconClass, form });
			}
		}
	}
}
