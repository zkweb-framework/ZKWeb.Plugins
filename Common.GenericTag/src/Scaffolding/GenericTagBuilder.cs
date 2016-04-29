using DryIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Repositories;
using ZKWeb.Plugins.Common.MenuPageBase.src;
using ZKWeb.Utils.Extensions;
using ZKWeb.Localize;
using ZKWeb.Web.Interfaces;
using ZKWeb.Web;
using ZKWeb.Database;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.GenericTag.src.Scaffolding {
	/// <summary>
	/// 通用标签构建器
	/// 使用时需要继承，例子
	/// [ExportMany]
	/// public class ExampleTag : GenericTagBuilder {
	///		public override string Name { get { return "ExampleTag"; } }
	/// }
	/// </summary>
	public abstract class GenericTagBuilder :
		GenericListForAdminSettings<Database.GenericTag, GenericTagBuilder> {
		/// <summary>
		/// 标签类型，默认使用名称（除去空格）
		/// </summary>
		public virtual string Type { get { return Name.Replace(" ", ""); } }
		/// <summary>
		/// 使用的权限
		/// </summary>
		public override string Privilege { get { return "TagManage:" + Type; } }
		/// <summary>
		/// 所属分组
		/// </summary>
		public override string Group { get { return "TagManage"; } }
		/// <summary>
		/// 分组图标
		/// </summary>
		public override string GroupIcon { get { return "fa fa-tags"; } }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public override string IconClass { get { return "fa fa-tags"; } }
		/// <summary>
		/// Url地址
		/// </summary>
		public override string Url { get { return "/admin/settings/generic_tag/" + Type.ToLower(); } }
		/// <summary>
		/// 添加使用的Url地址
		/// </summary>
		public virtual string AddUrl { get { return Url + "/add"; } }
		/// <summary>
		/// 编辑使用的Url地址
		/// </summary>
		public virtual string EditUrl { get { return Url + "/edit"; } }
		/// <summary>
		/// 批量操作使用的Url地址
		/// </summary>
		public virtual string BatchUrl { get { return Url + "/batch"; } }

		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected override IAjaxTableCallback<Database.GenericTag> GetTableCallback() {
			return new TableCallback(this);
		}

		/// <summary>
		/// 添加标签
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			return EditAction();
		}

		/// <summary>
		/// 编辑标签
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 处理表单绑定或提交
			var form = new Form(Type);
			var request = HttpContextUtils.CurrentContext.Request;
			if (request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.admin/generic_edit.html", new { form });
			}
		}

		/// <summary>
		/// 批量操作标签
		/// 目前支持批量删除，恢复，永久删除
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 拒绝处理非ajax提交的请求，防止跨站攻击
			var request = HttpContextUtils.CurrentContext.Request;
			if (!request.IsAjaxRequest()) {
				throw new HttpException(403, new T("Non ajax request batch action is not secure"));
			}
			// 获取参数
			var actionName = request.Get<string>("action");
			var json = HttpContextUtils.CurrentContext.Request.Get<string>("json");
			var idList = JsonConvert.DeserializeObject<IList<object>>(json);
			// 检查是否所有Id都属于指定的类型，防止越权操作
			var isAllTagTypeMatched = UnitOfWork.ReadRepository<GenericTagRepository, bool>(r => {
				return r.IsAllTagsTypeEqualTo(idList, Type);
			});
			if (!isAllTagTypeMatched) {
				throw new HttpException(403, new T("Try to access tag that type not matched"));
			}
			// 执行批量操作
			var message = UnitOfWork.WriteData<Database.GenericTag, string>(r => {
				if (actionName == "delete_forever") {
					r.BatchDeleteForever(idList);
					return new T("Batch Delete Forever Successful");
				} else if (actionName == "delete") {
					r.BatchDelete(idList);
					return new T("Batch Delete Successful");
				} else if (actionName == "recover") {
					r.BatchRecover(idList);
					return new T("Batch Recover Successful");
				} else {
					throw new HttpException(404, string.Format(new T("Action {0} not exist"), actionName));
				}
			});
			return new JsonResult(new { message });
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public override void OnWebsiteStart() {
			base.OnWebsiteStart();
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(AddUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.GET, EditAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.POST, EditAction);
			controllerManager.RegisterAction(BatchUrl, HttpMethods.POST, BatchAction);
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.GenericTag> {
			/// <summary>
			/// 标签构建器
			/// </summary>
			public GenericTagBuilder Builder { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public TableCallback(GenericTagBuilder builder) {
				Builder = builder;
			}

			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditAction(
					Builder.Type, Builder.EditUrl, dialogParameters: new { size = "size-wide" });
				table.MenuItems.AddAddAction(
					Builder.Type, Builder.AddUrl, dialogParameters: new { size = "size-wide" });
				searchBar.KeywordPlaceHolder = "Name/Remark";
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddAction(
					Builder.Type, Builder.AddUrl, dialogParameters: new { size = "size-wide" });
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericTag> query) {
				// 在第一页显示所有分类
				request.PageIndex = 0;
				request.PageSize = 0x7ffffffe;
				// 提供类型给其他回调
				request.Conditions["Type"] = Builder.Type;
				// 按类型
				query = query.Where(q => q.Type == Builder.Type);
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键词
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q => q.Name.Contains(request.Keyword) || q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericTag> query) {
				// 默认按显示顺序排列
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择字段
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<Database.GenericTag, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Name"] = pair.Key.Name;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["DisplayOrder"] = pair.Key.DisplayOrder;
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "45%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditAction(
					Builder.Type, Builder.EditUrl, dialogParameters: new { size = "size-wide" });
				idColumn.AddDivider();
				idColumn.AddDeleteActions(
					request, typeof(Database.GenericTag), Builder.Type, Builder.BatchUrl);
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<Database.GenericTag, Form> {
			/// <summary>
			/// 标签类型
			/// </summary>
			public string Type { get; set; }
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100)]
			[TextBoxField("Name", "Name")]
			public string Name { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="type">标签类型</param>
			public Form(string type) {
				Type = type;
			}

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, Database.GenericTag bindFrom) {
				if (bindFrom.Id > 0 && bindFrom.Type != Type) {
					// 检查类型，防止越权操作
					throw new HttpException(403, new T("Try to access tag that type not matched"));
				}
				Name = bindFrom.Name;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, Database.GenericTag saveTo) {
				if (saveTo.Id <= 0) {
					// 添加时
					saveTo.Type = Type;
					saveTo.CreateTime = DateTime.UtcNow;
				} else if (saveTo.Id > 0 && saveTo.Type != Type) {
					// 编辑时检查类型，防止越权操作
					throw new HttpException(403, new T("Try to access tag that type not matched"));
				}
				saveTo.Name = Name;
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
