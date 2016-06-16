using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.MenuPage.src.Scaffolding {
	/// <summary>
	/// 带单个表单的菜单页面构建器
	/// 需要再次经过包装，请勿直接使用
	/// </summary>
	public abstract class FormMenuPageBuilder : SimpleMenuPageBuilder {
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
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, RequiredPrivileges);
			// 处理绑定和提交
			var form = GetForm();
			if (HttpManager.CurrentContext.Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult(TemplatePath, new { title = Name, iconClass = IconClass, form });
			}
		}
	}
}
