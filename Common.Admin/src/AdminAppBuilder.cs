using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 后台应用构建器
	/// 支持自动生成列表和增删查改页面（Scaffold，半自动）
	/// 例子
	///	[ExportMany]
	///	public class TestDataManageApp : AdminAppBuilder[TestData, TestDataManageApp] {
	///		public override string Name { get { return "TestData Manage"; } }
	///		public override string Url { get { return "/admin/test_data"; } }
	///		public override string TileClass { get { return "tile bg-blue-hoki"; } }
	///		public override string IconClass { get { return "fa fa-legal"; } }
	///		protected override IAjaxTableCallback<TestData> GetTableCallback() { return new TableCallback(); }
	///		protected override FormBuilder GetAddForm() { return new EditForm(); }
	///		protected override FormBuilder GetEditForm() { return new EditForm(); }
	///		public class TableCallback : IAjaxTableCallback[TestData] { /* 实现函数 */ }
	///		public class EditForm : DataEditFormBuilder[TestData, EditForm] { /* 实现函数 */ }
	/// }
	/// </summary>
	/// <typeparam name="TData">管理的数据类型</typeparam>
	/// <typeparam name="TApp">继承类自身的类型</typeparam>
	[ExportMany]
	public abstract class AdminAppBuilder<TData, TApp> : AdminApp, IAdminAppBuilder
		where TData : class {
		/// <summary>
		/// 获取搜索结果的Url
		/// </summary>
		public virtual string SearchUrl { get { return Url + "/search"; } }
		/// <summary>
		/// 添加数据的Url
		/// </summary>
		public virtual string AddUrl { get { return Url + "/add"; } }
		/// <summary>
		/// 编辑数据的Url
		/// </summary>
		public virtual string EditUrl { get { return Url + "/edit"; } }
		/// <summary>
		/// 批量操作的Url
		/// </summary>
		public virtual string BatchUrl { get { return Url + "/batch"; } }
		/// <summary>
		/// 表格Id
		/// </summary>
		public virtual string TableId { get { return typeof(TData).Name + "List"; } }
		/// <summary>
		/// 显示列表页时引用的Css文件路径
		/// </summary>
		protected virtual List<string> IncludeCss { get; set; }
		/// <summary>
		/// 显示列表页时引用的Js文件路径
		/// </summary>
		protected virtual List<string> IncludeJs { get; set; }
		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected abstract IAjaxTableCallback<TData> GetTableCallback();
		/// <summary>
		/// 获取添加表单
		/// </summary>
		protected abstract IModelFormBuilder GetAddForm();
		/// <summary>
		/// 获取编辑表单
		/// </summary>
		/// <returns></returns>
		protected abstract IModelFormBuilder GetEditForm();

		/// <summary>
		/// 初始化
		/// </summary>
		public AdminAppBuilder() {
			IncludeCss = new List<string>();
			IncludeJs = new List<string>();
		}

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult ListAction() {
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = TableId;
			table.Target = SearchUrl;
			// 表格回调，内置+使用Ioc注册的额外回调
			var callbacks = new List<IAjaxTableCallback<TData>>() { GetTableCallback() };
			callbacks.AddRange(Application.Ioc.ResolveMany<IAjaxTableCallbackFor<TData, TApp>>());
			// 搜索栏构建器
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			callbacks.ForEach(s => s.OnBuildTable(table, searchBar));
			return new TemplateResult("common.admin/generic_list.html", new {
				includeCss = IncludeCss,
				includeJs = IncludeJs,
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
			// 获取参数并转换到搜索请求
			var json = HttpContext.Current.Request.GetParam<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 表格回调，内置+使用Ioc注册的额外回调
			var callbacks = new List<IAjaxTableCallback<TData>>() { GetTableCallback() };
			callbacks.AddRange(Application.Ioc.ResolveMany<IAjaxTableCallbackFor<TData, TApp>>());
			// 构建搜索回应
			var response = AjaxTableSearchResponse.FromRequest(request, callbacks);
			return new JsonResult(response);
		}

		/// <summary>
		/// 添加页和添加请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			var form = GetAddForm();
			var request = HttpContext.Current.Request;
			var attribute = form.GetFormAttribute();
			attribute.Action = attribute.Action ?? request.Url.PathAndQuery;
			if (request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit()); // 提交表单
			} else {
				form.Bind(); // 绑定表单
				return new TemplateResult("common.admin/generic_add.html", new { form });
			}
		}

		/// <summary>
		/// 编辑页和编辑请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			var form = GetEditForm();
			var request = HttpContext.Current.Request;
			var attribute = form.GetFormAttribute();
			attribute.Action = attribute.Action ?? request.Url.PathAndQuery;
			if (request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit()); // 提交表单
			} else {
				form.Bind(); // 绑定表单
				return new TemplateResult("common.admin/generic_edit.html", new { form });
			}
		}

		/// <summary>
		/// 批量操作请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取管理的数据类型
		/// </summary>
		/// <returns></returns>
		Type IAdminAppBuilder.GetDataType() {
			return typeof(TData);
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, ListAction);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.GET, EditAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.POST, EditAction);
			controllerManager.RegisterAction(BatchUrl, HttpMethods.POST, BatchAction);
		}
	}
}
