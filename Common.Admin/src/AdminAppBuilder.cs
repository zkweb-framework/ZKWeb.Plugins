using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
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
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 后台应用构建器
	/// 支持自动生成列表和增删查改页面（Scaffold，半自动）
	/// 例子
	///	[ExportMany]
	///	public class TestDataManageApp : AdminAppBuilder[TestData, TestDataManageApp] {
	///		public override string Name { get { return "TestData Manage"; } }
	///		public override string Url { get { return "/admin/test_data"; } }
	///		protected override IAjaxTableCallback<TestData> GetTableCallback() { return new TableCallback(); }
	///		protected override FormBuilder GetAddForm() { return new Form(); }
	///		protected override FormBuilder GetEditForm() { return new Form(); }
	///		public class TableCallback : IAjaxTableCallback[TestData] { /* 实现函数 */ }
	///		public class Form : DataEditFormBuilder[TestData, Form] { /* 实现函数 */ }
	/// }
	/// </summary>
	/// <typeparam name="TData">管理的数据类型</typeparam>
	/// <typeparam name="TApp">继承类自身的类型</typeparam>
	[ExportMany]
	public abstract class AdminAppBuilder<TData, TApp> :
		AdminApp, IAdminAppBuilder, IPrivilegesProvider
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
		/// 类型名称
		/// </summary>
		public virtual string TypeName { get { return typeof(TData).Name; } }
		/// <summary>
		/// 表格Id
		/// </summary>
		public virtual string TableId { get { return TypeName + "List"; } }
		/// <summary>
		/// 查看权限
		/// </summary>
		public virtual string ViewPrivilege { get { return Name + ":View"; } }
		/// <summary>
		/// 编辑权限
		/// </summary>
		public virtual string EditPrivilege { get { return Name + ":Edit"; } }
		/// <summary>
		/// 删除权限
		/// </summary>
		public virtual string DeletePrivilege { get { return Name + ":Delete"; } }
		/// <summary>
		/// 永久删除权限
		/// </summary>
		public virtual string DeleteForeverPrivilege { get { return Name + ":DeleteForever"; } }
		/// <summary>
		/// 默认需要拥有管理员权限
		/// </summary>
		public override UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 默认需要拥有查看权限
		/// </summary>
		public override string[] RequiredPrivileges { get { return new[] { ViewPrivilege }; } }
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
		/// 批量操作对应的函数
		/// </summary>
		protected virtual Dictionary<string, Func<IActionResult>> BatchActions { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AdminAppBuilder() {
			IncludeCss = new List<string>();
			IncludeJs = new List<string>();
			BatchActions = new Dictionary<string, Func<IActionResult>>();
			BatchActions["delete_forever"] = BatchActionForDeleteForever;
			if (IsRecyclable<TData>.Value) {
				BatchActions["delete"] = BatchActionForDelete;
				BatchActions["recover"] = BatchActionForRecover;
			}
		}

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
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
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
				// 检查权限
				PrivilegesChecker.Check(AllowedUserTypes, EditPrivilege);
				// 提交表单
				return new JsonResult(form.Submit());
			} else {
				// 检查权限
				PrivilegesChecker.Check(AllowedUserTypes, ViewPrivilege);
				// 绑定表单
				form.Bind();
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
				// 检查权限
				PrivilegesChecker.Check(AllowedUserTypes, EditPrivilege);
				// 提交表单
				return new JsonResult(form.Submit());
			} else {
				// 检查权限
				PrivilegesChecker.Check(AllowedUserTypes, ViewPrivilege);
				// 绑定表单
				form.Bind();
				return new TemplateResult("common.admin/generic_edit.html", new { form });
			}
		}

		/// <summary>
		/// 批量操作请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			var request = HttpContext.Current.Request;
			if (!request.IsAjaxRequest()) {
				// 非ajax提交的请求有跨站攻击的危险，这里拒绝处理
				throw new HttpException(403, new T("Non ajax request batch action is not secure"));
			}
			var actionName = request.GetParam<string>("action");
			var action = BatchActions.GetOrDefault(actionName);
			if (action == null) {
				// 找不到对应的操作
				throw new HttpException(404, string.Format(new T("Action {0} not exist"), actionName));
			}
			return action();
		}

		/// <summary>
		/// 批量删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDelete() {
			PrivilegesChecker.Check(AllowedUserTypes, DeletePrivilege);
			var json = HttpContext.Current.Request.GetParam<string>("json");
			var idList = JsonConvert.DeserializeObject<IList<long>>(json);
			var deleter = Application.Ioc.Resolve<GenericDataDeleter>();
			deleter.BatchDelete<TData>(idList);
			return new JsonResult(new { message = new T("Batch Delete Successful") });
		}

		/// <summary>
		/// 批量恢复
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForRecover() {
			PrivilegesChecker.Check(AllowedUserTypes, DeletePrivilege);
			var json = HttpContext.Current.Request.GetParam<string>("json");
			var idList = JsonConvert.DeserializeObject<IList<long>>(json);
			var deleter = Application.Ioc.Resolve<GenericDataDeleter>();
			deleter.BatchRecover<TData>(idList);
			return new JsonResult(new { message = new T("Batch Recover Successful") });
		}

		/// <summary>
		/// 批量永久删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDeleteForever() {
			PrivilegesChecker.Check(AllowedUserTypes, DeleteForeverPrivilege);
			var json = HttpContext.Current.Request.GetParam<string>("json");
			var idList = JsonConvert.DeserializeObject<IList<long>>(json);
			var deleter = Application.Ioc.Resolve<GenericDataDeleter>();
			deleter.BatchDeleteForever<TData>(idList);
			return new JsonResult(new { message = new T("Batch Delete Forever Successful") });
		}

		/// <summary>
		/// 获取管理的数据类型
		/// </summary>
		/// <returns></returns>
		Type IAdminAppBuilder.GetDataType() {
			return typeof(TData);
		}

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<string> GetPrivileges() {
			yield return ViewPrivilege;
			yield return EditPrivilege;
			if (IsRecyclable<TData>.Value) {
				yield return DeletePrivilege;
			}
			yield return DeleteForeverPrivilege;
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public override void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.GET, EditAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.POST, EditAction);
			controllerManager.RegisterAction(BatchUrl, HttpMethods.POST, BatchAction);
		}
	}
}
