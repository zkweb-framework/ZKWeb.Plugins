using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Common.Admin.src.Scaffolding {
	/// <summary>
	/// 增删查改页面的构建器
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public abstract class CrudPageBuilder<TData> :
		ICrudPageBuilder, IPrivilegesProvider, IWebsiteStartHandler
		where TData : class {
		/// <summary>
		/// 名称
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// 基础链接
		/// </summary>
		public abstract string Url { get; }
		/// <summary>
		/// 获取搜索结果的Url
		/// </summary>
		public virtual string SearchUrl { get { return Url + "/search"; } }
		/// <summary>
		/// 添加数据的Url
		/// 返回空值时表示不支持添加数据
		/// </summary>
		public virtual string AddUrl { get { return Url + "/add"; } }
		/// <summary>
		/// 编辑数据的Url
		/// 返回空值时表示不支持编辑数据
		/// </summary>
		public virtual string EditUrl { get { return Url + "/edit"; } }
		/// <summary>
		/// 批量操作的Url
		/// 返回空值时表示不支持批量操作数据
		/// </summary>
		public virtual string BatchUrl { get { return Url + "/batch"; } }
		/// <summary>
		/// 允许操作的用户类型
		/// </summary>
		public abstract UserTypes[] AllowedUserTypes { get; }
		/// <summary>
		/// 查看需要的权限
		/// </summary>
		public virtual string[] ViewPrivileges { get { return new[] { Name + ":View" }; } }
		/// <summary>
		/// 编辑需要的权限
		/// </summary>
		public virtual string[] EditPrivileges { get { return new[] { Name + ":Edit" }; } }
		/// <summary>
		/// 删除需要的权限
		/// </summary>
		public virtual string[] DeletePrivileges { get { return new[] { Name + ":Delete" }; } }
		/// <summary>
		/// 永久删除需要的权限
		/// </summary>
		public virtual string[] DeleteForeverPrivilege { get { return new[] { Name + ":DeleteForever" }; } }
		/// <summary>
		/// 数据类型
		/// </summary>
		public virtual Type DataType { get { return typeof(TData); } }
		/// <summary>
		/// 数据类型的名称
		/// </summary>
		public virtual string DataTypeName { get { return typeof(TData).Name; } }
		/// <summary>
		/// 列表页中使用的表格Id
		/// </summary>
		public virtual string ListTableId { get { return DataTypeName + "List"; } }
		/// <summary>
		/// 列表页的模板路径
		/// </summary>
		public abstract string ListTemplatePath { get; }
		/// <summary>
		/// 添加页的模板路径
		/// </summary>
		public abstract string AddTemplatePath { get; }
		/// <summary>
		/// 编辑页的模板路径
		/// </summary>
		public abstract string EditTemplatePath { get; }
		/// <summary>
		/// 传给模板页的引用的样式文件列表
		/// </summary>
		protected virtual IList<string> IncludeCss { get; set; }
		/// <summary>
		/// 传给模板页的引用的脚本文件列表
		/// </summary>
		protected virtual IList<string> IncludeJs { get; set; }
		/// <summary>
		/// 传给模板页的附加数据
		/// </summary>
		protected virtual IDictionary<string, object> ExtraTemplateArguments { get; set; }
		/// <summary>
		/// 批量操作对应的函数
		/// </summary>
		protected virtual Dictionary<string, Func<IActionResult>> BatchActions { get; set; }
		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected abstract IAjaxTableCallback<TData> GetTableCallback();
		/// <summary>
		/// 获取添加数据使用的表单
		/// </summary>
		/// <returns></returns>
		protected abstract IModelFormBuilder GetAddForm();
		/// <summary>
		/// 获取编辑数据使用的表单
		/// </summary>
		/// <returns></returns>
		protected abstract IModelFormBuilder GetEditForm();

		/// <summary>
		/// 初始化
		/// </summary>
		public CrudPageBuilder() {
			IncludeCss = new List<string>();
			IncludeJs = new List<string>();
			ExtraTemplateArguments = new Dictionary<string, object>();
			BatchActions = new Dictionary<string, Func<IActionResult>>();
			BatchActions["delete_forever"] = BatchActionForDeleteForever;
			if (RecyclableTrait.For<TData>().IsRecyclable) {
				BatchActions["delete"] = BatchActionForDelete;
				BatchActions["recover"] = BatchActionForRecover;
			}
		}

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult Action() {
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, ViewPrivileges);
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = ListTableId;
			table.Target = SearchUrl;
			// 表格回调，内置+使用Ioc注册的扩展回调
			var callbacks = GetTableCallback().WithExtensions();
			// 搜索栏构建器
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			callbacks.ForEach(s => s.OnBuildTable(table, searchBar));
			return new TemplateResult(ListTemplatePath, new {
				title = new T(Name),
				includeCss = IncludeCss,
				includeJs = IncludeJs,
				extra = ExtraTemplateArguments,
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
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, ViewPrivileges);
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
		/// 添加页和添加请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			var form = GetAddForm();
			var request = HttpContextUtils.CurrentContext.Request;
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			if (request.HttpMethod == HttpMethods.POST) {
				// 检查权限
				privilegeManager.Check(AllowedUserTypes, EditPrivileges);
				// 提交表单
				return new JsonResult(form.Submit());
			} else {
				// 检查权限
				privilegeManager.Check(AllowedUserTypes, ViewPrivileges);
				// 绑定表单
				form.Bind();
				return new TemplateResult(AddTemplatePath, new { form });
			}
		}

		/// <summary>
		/// 编辑页和编辑请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			var form = GetEditForm();
			var request = HttpContextUtils.CurrentContext.Request;
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			if (request.HttpMethod == HttpMethods.POST) {
				// 检查权限
				privilegeManager.Check(AllowedUserTypes, EditPrivileges);
				// 提交表单
				return new JsonResult(form.Submit());
			} else {
				// 检查权限
				privilegeManager.Check(AllowedUserTypes, ViewPrivileges);
				// 绑定表单
				form.Bind();
				return new TemplateResult(EditTemplatePath, new { form });
			}
		}

		/// <summary>
		/// 批量操作请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			HttpRequestChecker.RequieAjaxRequest();
			var request = HttpContextUtils.CurrentContext.Request;
			var actionName = request.Get<string>("action");
			var action = BatchActions.GetOrDefault(actionName);
			if (action == null) {
				// 找不到对应的操作
				throw new HttpException(404, string.Format(new T("Action {0} not exist"), actionName));
			}
			return action();
		}

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// </summary>
		/// <returns></returns>
		protected virtual IList<object> GetBatchActionIds() {
			var json = HttpContextUtils.CurrentContext.Request.Get<string>("json");
			var obj = JsonConvert.DeserializeObject<JToken>(json);
			if (obj is JArray) {
				// [ id列表 ]
				return obj.Values<object>().ToList();
			} else if (obj is JObject) {
				// { ids: [ id列表 ], ... }
				return ((JObject)obj).GetValue("ids").Values<object>().ToList();
			}
			throw new ArgumentException(string.Format(
				"get batch action ids failed, unknown format: {0}", json));
		}

		/// <summary>
		/// 批量删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDelete() {
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, DeletePrivileges);
			// 批量删除
			UnitOfWork.WriteData<TData>(r => r.BatchDelete(GetBatchActionIds()));
			return new JsonResult(new { message = new T("Batch Delete Successful") });
		}

		/// <summary>
		/// 批量恢复
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForRecover() {
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, DeletePrivileges);
			// 批量恢复
			UnitOfWork.WriteData<TData>(r => r.BatchRecover(GetBatchActionIds()));
			return new JsonResult(new { message = new T("Batch Recover Successful") });
		}

		/// <summary>
		/// 批量永久删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDeleteForever() {
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, DeleteForeverPrivilege);
			// 批量永久删除
			UnitOfWork.WriteData<TData>(r => r.BatchDeleteForever(GetBatchActionIds()));
			return new JsonResult(new { message = new T("Batch Delete Forever Successful") });
		}

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<string> GetPrivileges() {
			// 注册查看权限
			foreach (var privilege in ViewPrivileges) {
				yield return privilege;
			}
			// 允许添加或编辑数据时注册编辑权限
			if (!string.IsNullOrEmpty(AddUrl) || !string.IsNullOrEmpty(EditUrl)) {
				foreach (var privilege in EditPrivileges) {
					yield return privilege;
				}
			}
			// 允许批量操作数据时注册删除权限
			if (!string.IsNullOrEmpty(BatchUrl)) {
				// 可回收时注册删除权限
				if (RecyclableTrait.For<TData>().IsRecyclable) {
					foreach (var privilege in DeletePrivileges) {
						yield return privilege;
					}
				}
				// 注册永久删除权限
				foreach (var privilege in DeleteForeverPrivilege) {
					yield return privilege;
				}
			}
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			// 注册列表页和搜索接口
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
			if (!string.IsNullOrEmpty(AddUrl)) {
				// 注册添加页和添加接口
				controllerManager.RegisterAction(AddUrl, HttpMethods.GET, AddAction);
				controllerManager.RegisterAction(AddUrl, HttpMethods.POST, AddAction);
			}
			if (!string.IsNullOrEmpty(EditUrl)) {
				// 注册编辑页和编辑接口
				controllerManager.RegisterAction(EditUrl, HttpMethods.GET, EditAction);
				controllerManager.RegisterAction(EditUrl, HttpMethods.POST, EditAction);
			}
			if (!string.IsNullOrEmpty(BatchUrl)) {
				// 注册批量操作接口
				controllerManager.RegisterAction(BatchUrl, HttpMethods.POST, BatchAction);
			}
		}
	}
}
