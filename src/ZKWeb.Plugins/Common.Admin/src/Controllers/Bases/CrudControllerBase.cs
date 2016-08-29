using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeProviders;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Filters;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers.Bases {
	/// <summary>
	/// 用于增删查改数据的控制器的基础类
	/// </summary>
	public abstract class CrudControllerBase<TEntity, TPrimaryKey> :
		ControllerBase,
		ICrudController, IPrivilegesProvider, IWebsiteStartHandler
		where TEntity : class, IEntity<TPrimaryKey> {
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
		/// 增删查改操作需要的用户类型
		/// </summary>
		public abstract Type RequiredUserType { get; }
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
		public virtual string[] DeleteForeverPrivileges { get { return new[] { Name + ":DeleteForever" }; } }
		/// <summary>
		/// 数据类型
		/// </summary>
		public virtual Type EntityType { get { return typeof(TEntity); } }
		/// <summary>
		/// 数据类型的名称
		/// </summary>
		public virtual string EntityTypeName { get { return typeof(TEntity).Name; } }
		/// <summary>
		/// 列表页中使用的表格Id
		/// </summary>
		public virtual string ListTableId { get { return EntityTypeName + "List"; } }
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
		/// 批量操作对应的函数和权限列表
		/// </summary>
		protected virtual IDictionary<string, Tuple<Func<IActionResult>, string[]>>
			BatchActions { get; set; }
		/// <summary>
		/// 是否在增删查改操作中使用事务
		/// </summary>
		protected virtual bool UseTransaction { get { return false; } }
		/// <summary>
		/// 是否关心实体的所属用户
		/// 等于true时会启用所属用户使用的查询和保存过滤器，并且在批量操作时进行检查
		/// </summary>
		protected abstract bool ConcernEntityOwnership { get; }
		/// <summary>
		/// 获取表格处理器
		/// </summary>
		/// <returns></returns>
		protected abstract IAjaxTableHandler<TEntity, TPrimaryKey> GetTableHandler();
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
		/// 类型特征
		/// </summary>
		protected static class TypeTrait {
			/// <summary>
			/// 类型包含已删除标记
			/// </summary>
			public readonly static bool HaveDeleted =
				typeof(IHaveDeleted).GetTypeInfo().IsAssignableFrom(typeof(TEntity));
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public CrudControllerBase() {
			IncludeCss = new List<string>();
			IncludeJs = new List<string>();
			ExtraTemplateArguments = new Dictionary<string, object>();
			BatchActions = new Dictionary<string, Tuple<Func<IActionResult>, string[]>>();
			BatchActions["delete_forever"] = Tuple.Create(
				new Func<IActionResult>(BatchActionForDeleteForever), DeleteForeverPrivileges);
			if (TypeTrait.HaveDeleted) {
				BatchActions["delete"] = Tuple.Create(
					new Func<IActionResult>(BatchActionForDelete), DeletePrivileges);
				BatchActions["recover"] = Tuple.Create(
					new Func<IActionResult>(BatchActionForRecover), DeletePrivileges);
			}
		}

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult Action() {
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = ListTableId;
			table.Target = SearchUrl;
			// 表格处理器，内置+使用Ioc注册的扩展回调
			var handlers = GetTableHandler().WithExtraHandlers();
			// 搜索栏构建器
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			handlers.ForEach(s => s.BuildTable(table, searchBar));
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
			// 获取参数并转换到搜索请求
			var json = Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 表格处理器，内置+使用Ioc注册的扩展回调
			var handlers = GetTableHandler().WithExtraHandlers();
			// 构建搜索回应
			var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
			var response = request.BuildResponse(handlers);

			return new JsonResult(response);
		}

		/// <summary>
		/// 添加页和添加请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			var form = GetAddForm();
			if (Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult(AddTemplatePath,
					new { form, extra = ExtraTemplateArguments });
			}
		}

		/// <summary>
		/// 编辑页和编辑请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			var form = GetEditForm();
			if (Request.Method == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult(EditTemplatePath,
					new { form, extra = ExtraTemplateArguments });
			}
		}

		/// <summary>
		/// 批量操作请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			// 防跨站攻击
			this.RequireAjaxRequest();
			var actionName = Request.Get<string>("action");
			var action = BatchActions.GetOrDefault(actionName);
			if (action == null) {
				// 找不到对应的操作
				throw new NotFoundException(string.Format(new T("Action {0} not exist"), actionName));
			}
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(RequiredUserType, action.Item2);
			return action.Item1();
		}

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// </summary>
		/// <returns></returns>
		protected virtual IList<TPrimaryKey> GetBatchActionIds() {
			var json = Request.Get<string>("json");
			var obj = JsonConvert.DeserializeObject<JToken>(json);
			IList<TPrimaryKey> result;
			if (obj is JArray) {
				// [ id列表 ]
				result = obj.Values<object>()
					.Select(o => o.ConvertOrDefault<TPrimaryKey>()).ToList();
			} else if (obj is JObject) {
				// { ids: [ id列表 ], ... }
				result = ((JObject)obj).GetValue("ids").Values<object>()
					.Select(o => o.ConvertOrDefault<TPrimaryKey>()).ToList();
			} else {
				throw new ArgumentException(string.Format(
					"get batch action ids failed, unknown format: {0}", json));
			}
			return result;
		}

		/// <summary>
		/// 批量删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDelete() {
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			service.BatchSetDeleted(GetBatchActionIds(), true);
			return new JsonResult(new { message = new T("Batch Delete Successful") });
		}

		/// <summary>
		/// 批量恢复
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForRecover() {
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			service.BatchSetDeleted(GetBatchActionIds(), false);
			return new JsonResult(new { message = new T("Batch Recover Successful") });
		}

		/// <summary>
		/// 批量永久删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchActionForDeleteForever() {
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			service.BatchDeleteForever(GetBatchActionIds());
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
				// 可标记删除时注册删除权限
				if (TypeTrait.HaveDeleted) {
					foreach (var privilege in DeletePrivileges) {
						yield return privilege;
					}
				}
				// 注册永久删除权限
				foreach (var privilege in DeleteForeverPrivileges) {
					yield return privilege;
				}
			}
		}

		/// <summary>
		/// 对处理函数进行包装
		/// </summary>
		/// <param name="action">处理函数</param>
		/// <param name="privilege">要求的权限列表</param>
		/// <returns></returns>
		protected virtual Func<IActionResult> WrapAction(
			Func<IActionResult> action, params string[] privileges) {
			return () => {
				// 检查权限
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				privilegeManager.Check(RequiredUserType, privileges);
				// 使用工作单元包装处理函数
				// 指定使用事务时开启事务
				var uow = Application.Ioc.Resolve<IUnitOfWork>();
				var useTransaction = UseTransaction;
				IActionResult result;
				using (uow.Scope()) {
					if (useTransaction) {
						// 开始事务
						uow.Context.BeginTransaction();
					}
					if (ConcernEntityOwnership) {
						// 启用所属用户使用的查询和操作过滤器
						var filter = new OwnerFilter();
						using (uow.EnableQueryFilter(filter))
						using (uow.EnableOperationFilter(filter)) {
							result = action();
						}
					} else {
						result = action();
					}
					if (useTransaction) {
						// 结束事务
						uow.Context.FinishTransaction();
					}
				}
				return result;
			};
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			// 注册列表页和搜索接口
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(
				Url, HttpMethods.GET, WrapAction(Action, ViewPrivileges));
			controllerManager.RegisterAction(
				SearchUrl, HttpMethods.POST, WrapAction(SearchAction, ViewPrivileges));
			if (!string.IsNullOrEmpty(AddUrl)) {
				// 注册添加页和添加接口
				controllerManager.RegisterAction(
					AddUrl, HttpMethods.GET, WrapAction(AddAction, ViewPrivileges));
				controllerManager.RegisterAction(
					AddUrl, HttpMethods.POST, WrapAction(AddAction, EditPrivileges));
			}
			if (!string.IsNullOrEmpty(EditUrl)) {
				// 注册编辑页和编辑接口
				controllerManager.RegisterAction(
					EditUrl, HttpMethods.GET, WrapAction(EditAction, ViewPrivileges));
				controllerManager.RegisterAction(
					EditUrl, HttpMethods.POST, WrapAction(EditAction, EditPrivileges));
			}
			if (!string.IsNullOrEmpty(BatchUrl)) {
				// 注册批量操作接口，具体权限需要在里面检查
				controllerManager.RegisterAction(
					BatchUrl, HttpMethods.POST, WrapAction(BatchAction));
			}
		}
	}
}
