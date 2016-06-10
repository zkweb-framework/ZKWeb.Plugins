using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Web.Interfaces;
using ZKWeb.Localize;
using ZKWeb.Web;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.MenuPageBase.src.Scaffolding {
	/// <summary>
	/// 用于快速添加菜单页中只带有一个数据列表的简单页面
	/// 这个抽象类需要再次继承，请勿直接使用
	/// 例子
	/// public abstract class GenericListForAdminSettings[TData] :
	///		GenericListForMenuPage[TData], IMenuProviderForAdminSettings { }
	/// [ExportMany] public class ExampleList : GenericListForAdminSettings[Data, ExampleList] { }
	/// </summary>
	/// <typeparam name="TData">列表中的数据类型</typeparam>
	public abstract class GenericListForMenuPage<TData> : GenericPageForMenuPage
		where TData : class {
		/// <summary>
		/// 获取搜索的Url
		/// </summary>
		public virtual string SearchUrl { get { return Url + "/search"; } }
		/// <summary>
		/// 表格Id
		/// </summary>
		public virtual string TableId { get { return Name.Replace(" ", "") + "List"; } }
		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected abstract IAjaxTableCallback<TData> GetTableCallback();

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = TableId;
			table.Target = SearchUrl;
			// 表格回调，内置+使用Ioc注册的扩展回调
			var callbacks = GetTableCallback().WithExtensions();
			// 搜索栏构建器
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			callbacks.ForEach(s => s.OnBuildTable(table, searchBar));
			return new TemplateResult(TemplatePath, new {
				title = new T(Name),
				iconClass = IconClass,
				table,
				searchBar
			});
		}

		/// <summary>
		/// 搜索请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult SearchAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 获取参数并转换到搜索请求
			var json = HttpContextUtils.CurrentContext.Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 表格回调，内置+使用Ioc注册的扩展回调
			var callbacks = GetTableCallback().WithExtensions();
			// 构建搜索回应
			var response = request.BuildResponseFromDatabase(callbacks);
			return new JsonResult(response);
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public override void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
		}
	}
}
