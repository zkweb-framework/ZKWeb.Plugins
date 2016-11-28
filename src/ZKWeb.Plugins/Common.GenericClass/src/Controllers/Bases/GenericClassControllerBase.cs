using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.GenericClass.src.Controllers.Bases {
	using GenericClass = Domain.Entities.GenericClass;

	/// <summary>
	/// 通用分类的增删查改控制器的基础类
	/// </summary>
	public abstract class GenericClassControllerBase<TController> :
		CrudAdminSettingsControllerBase<GenericClass, Guid>
		where TController : GenericClassControllerBase<TController>, new() {
		public virtual string Type { get { return Name.Replace(" ", ""); } }
		public override string Group { get { return "ClassManage"; } }
		public override string GroupIconClass { get { return "fa fa-list"; } }
		public override string IconClass { get { return "fa fa-list"; } }
		public override string Url { get { return "/admin/settings/generic_class/" + Type.ToLower(); } }
		public override string[] ViewPrivileges { get { return new[] { "ClassManage:" + Type }; } }
		public override string[] EditPrivileges { get { return ViewPrivileges; } }
		public override string[] DeletePrivileges { get { return ViewPrivileges; } }
		public override string[] DeleteForeverPrivileges { get { return ViewPrivileges; } }
		public override string ListTemplatePath { get { return "common.generic_class/class_list.html"; } }
		public override string EntityTypeName { get { return Type; } }
		protected override IAjaxTableHandler<GenericClass, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(Type); }
		protected override IModelFormBuilder GetEditForm() { return new Form(Type); }

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// </summary>
		/// <returns></returns>
		protected override IList<Guid> GetBatchActionIds() {
			// Id列表需要把顺序倒转，用于先删除子分类再删除上级分类
			var ids = base.GetBatchActionIds().Reverse().ToList();
			// 检查是否所有Id都属于指定的类型，防止越权操作
			var genericClassManager = Application.Ioc.Resolve<GenericClassManager>();
			if (!genericClassManager.IsAllClassesTypeEqualTo(ids, Type)) {
				throw new ForbiddenException(new T("Try to access class that type not matched"));
			}
			return ids;
		}

		/// <summary>
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<GenericClass, Guid> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				var app = new TController();
				var dialogParameters = new { size = "size-wide" };
				table.ExtraOptions["pageSize"] = int.MaxValue;
				table.ExtraOptions["hidePagination"] = true;
				table.MenuItems.AddToggleAllForAjaxTableTree("Level");
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditAction(app.Type, app.EditUrl, dialogParameters: dialogParameters);
				table.MenuItems.AddAddAction(app.Type, app.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: dialogParameters);
				table.MenuItems.AddRemoteModalForSelectedRow(
					new T("Add Same Level Class"), "fa fa-plus",
					new T("Add {0}", new T(app.Type)),
					app.AddUrl + "?parentId=<%-row.ParentId%>", dialogParameters);
				table.MenuItems.AddRemoteModalForSelectedRow(
					new T("Add Child Class"), "fa fa-plus",
					new T("Add {0}", new T(app.Type)),
					app.AddUrl + "?parentId=<%-row.Id%>", dialogParameters);
				searchBar.KeywordPlaceHolder = "Name/Remark";
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddAction(app.Type, app.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: dialogParameters);
				searchBar.BeforeItems.AddAddAction(app.Type, app.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: dialogParameters);
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<GenericClass> query) {
				// 提供类型给其他处理器
				var app = new TController();
				request.Conditions["Type"] = app.Type;
				// 按类型
				query = query.Where(q => q.Type == app.Type);
				// 按关键词
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q => q.Name.Contains(request.Keyword) || q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<GenericClass> query) {
				// 默认按显示顺序排列
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<GenericClass>> pairs) {
				// 按上下级关系重新生成数据列表
				var classMapping = pairs.ToDictionary(p => p.Entity.Id);
				var tree = TreeUtils.CreateTree(pairs, p => p,
					p => classMapping.GetOrDefault(p.Entity.Parent?.Id ?? Guid.Empty));
				pairs.Clear();
				foreach (var node in tree.EnumerateAllNodes().Skip(1)) {
					var pair = node.Value;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["ParentId"] = pair.Entity.Parent?.Id;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
					pair.Row["Level"] = node.GetParents().Count() - 1;
					pair.Row["NoChilds"] = !node.Childs.Any();
					pairs.Add(pair);
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<TController>(request);
				response.Columns.AddTreeNodeColumn("Name", "Level", "NoChilds");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				var dialogParameters = new { size = "size-wide" };
				if (!deleted) {
					actionColumn.AddEditActionFor<TController>(dialogParameters: dialogParameters);
					actionColumn.AddDeleteActionFor<TController>();
				} else {
					actionColumn.AddRecoverActionFor<TController>();
					actionColumn.AddDeleteForeverActionFor<TController>();
				}
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : EntityFormBuilder<GenericClass, Guid, Form> {
			/// <summary>
			/// 分类类型
			/// </summary>
			public string Type { get; set; }
			/// <summary>
			/// 上级分类名称
			/// </summary>
			[LabelField("ParentClass")]
			public string ParentClass { get; set; }
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100)]
			[TextBoxField("Name")]
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
			/// <param name="type">分类类型</param>
			public Form(string type) {
				Type = type;
			}

			/// <summary>
			/// 根据当前请求传入的parentId参数获取上级分类，不存在时返回null
			/// 这个函数只在添加时使用
			/// </summary>
			protected GenericClass GetParentClass() {
				var parentId = HttpManager.CurrentContext.Request.Get<Guid>("parentId");
				if (parentId == Guid.Empty) {
					return null;
				}
				var genericClassManager = Application.Ioc.Resolve<GenericClassManager>();
				var parent = genericClassManager.Get(parentId);
				if (parent == null) {
					return null;
				} else if (parent.Type != Type) {
					throw new ForbiddenException(new T("Try to access class that type not matched"));
				}
				return parent;
			}

			/// <summary>
			/// 从数据绑定表单
			/// </summary>
			protected override void OnBind(GenericClass bindFrom) {
				if (bindFrom.Id == Guid.Empty) {
					// 添加时
					var parent = GetParentClass();
					ParentClass = parent == null ? "" : parent.Name;
				} else {
					// 编辑时
					ParentClass = bindFrom.Parent == null ? "" : bindFrom.Parent.Name;
					// 检查类型，防止越权操作
					if (bindFrom.Type != Type) {
						throw new ForbiddenException(new T("Try to access class that type not matched"));
					}
				}
				Name = bindFrom.Name;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(GenericClass saveTo) {
				if (saveTo.Type == null) {
					// 添加时
					saveTo.Type = Type;
					saveTo.Parent = GetParentClass();
				} else if (saveTo.Type != Type) {
					// 编辑时检查类型，防止越权操作
					throw new ForbiddenException(new T("Try to access class that type not matched"));
				}
				saveTo.Name = Name;
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
