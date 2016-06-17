using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericClass.src.Repositories;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Localize;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.GenericClass.src.Scaffolding {
	/// <summary>
	/// 通用分类构建器
	/// </summary>
	/// <example>
	/// [ExportMany]
	/// public class ExampleClass : GenericClassBuilder {
	///		public override string Name { get { return "ExampleClass"; } }
	/// }
	/// </example>
	public abstract class GenericClassBuilder :
		AdminSettingsCrudPageBuilder<Database.GenericClass> {
		public virtual string Type { get { return Name.Replace(" ", ""); } }
		public override string Group { get { return "ClassManage"; } }
		public override string GroupIconClass { get { return "fa fa-list"; } }
		public override string IconClass { get { return "fa fa-list"; } }
		public override string Url { get { return "/admin/settings/generic_class/" + Type.ToLower(); } }
		public override string[] ViewPrivileges { get { return new[] { "ClassManage:" + Type }; } }
		public override string[] EditPrivileges { get { return ViewPrivileges; } }
		public override string[] DeletePrivileges { get { return ViewPrivileges; } }
		public override string[] DeleteForeverPrivilege { get { return ViewPrivileges; } }
		public override string ListTemplatePath { get { return "common.generic_class/class_list.html"; } }
		protected override IAjaxTableCallback<Database.GenericClass> GetTableCallback() {
			return new TableCallback(this);
		}
		protected override IModelFormBuilder GetAddForm() { return new Form(Type); }
		protected override IModelFormBuilder GetEditForm() { return new Form(Type); }

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// </summary>
		/// <returns></returns>
		protected override IList<object> GetBatchActionIds() {
			// 获取参数
			// 其中Id列表需要把顺序倒转，用于先删除子分类再删除上级分类
			var request = HttpManager.CurrentContext.Request;
			var actionName = request.Get<string>("action");
			var json = HttpManager.CurrentContext.Request.Get<string>("json");
			var ids = JsonConvert.DeserializeObject<IList<object>>(json).Reverse().ToList();
			// 检查是否所有Id都属于指定的类型，防止越权操作
			var isAllClassesTypeMatched = UnitOfWork.ReadRepository<GenericClassRepository, bool>(r => {
				return r.IsAllClassesTypeEqualTo(ids, Type);
			});
			if (!isAllClassesTypeMatched) {
				throw new HttpException(403, new T("Try to access class that type not matched"));
			}
			return ids;
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.GenericClass> {
			/// <summary>
			/// 分类构建器
			/// </summary>
			public GenericClassBuilder Builder { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public TableCallback(GenericClassBuilder builder) {
				Builder = builder;
			}

			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.ExtraOptions["pageSize"] = int.MaxValue;
				table.ExtraOptions["hidePagination"] = true;
				table.MenuItems.AddToggleAllForAjaxTableTree("Level");
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditAction(
					Builder.Type, Builder.EditUrl, dialogParameters: new { size = "size-wide" });
				table.MenuItems.AddAddAction(
					Builder.Type, Builder.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: new { size = "size-wide" });
				table.MenuItems.AddRemoteModalForSelectedRow(
					new T("Add Same Level Class"), "fa fa-plus",
					string.Format(new T("Add {0}"), new T(Builder.Type)),
					Builder.AddUrl + "?parentId=<%-row.ParentId%>", new { size = "size-wide" });
				table.MenuItems.AddRemoteModalForSelectedRow(
					new T("Add Child Class"), "fa fa-plus",
					string.Format(new T("Add {0}"), new T(Builder.Type)),
					Builder.AddUrl + "?parentId=<%-row.Id%>", new { size = "size-wide" });
				searchBar.KeywordPlaceHolder = "Name/Remark";
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddAction(
					Builder.Type, Builder.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: new { size = "size-wide" });
				searchBar.BeforeItems.AddAddAction(
					Builder.Type, Builder.AddUrl,
					name: new T("Add Top Level Class"), dialogParameters: new { size = "size-wide" });
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericClass> query) {
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
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericClass> query) {
				// 默认按显示顺序排列
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<EntityToTableRow<Database.GenericClass>> pairs) {
				// 按上下级关系重新生成数据列表
				var classMapping = pairs.ToDictionary(p => p.Entity.Id);
				var tree = TreeUtils.CreateTree(pairs,
					p => p, p => classMapping.GetOrDefault(p.Entity.Parent == null ? 0 : p.Entity.Parent.Id));
				pairs.Clear();
				foreach (var node in tree.EnumerateAllNodes().Skip(1)) {
					var pair = node.Value;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["ParentId"] = pair.Entity.Parent == null ? 0 : pair.Entity.Parent.Id;
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
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddTreeNodeColumn("Name", "Level", "NoChilds");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditAction(
					Builder.Type, Builder.EditUrl, dialogParameters: new { size = "size-wide" });
				idColumn.AddDivider();
				idColumn.AddDeleteActions(
					request, typeof(Database.GenericClass), Builder.Type, Builder.BatchUrl);
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : DataEditFormBuilder<Database.GenericClass, Form> {
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
			protected Database.GenericClass GetParentClass(DatabaseContext context) {
				var parentId = HttpManager.CurrentContext.Request.Get<long>("parentId");
				if (parentId <= 0) {
					return null;
				}
				var parent = context.Get<Database.GenericClass>(c => c.Id == parentId);
				if (parent == null) {
					return null;
				} else if (parent.Type != Type) {
					throw new HttpException(403, new T("Try to access class that type not matched"));
				}
				return parent;
			}

			/// <summary>
			/// 从数据绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, Database.GenericClass bindFrom) {
				if (bindFrom.Id <= 0) {
					// 添加时
					var parent = GetParentClass(context);
					ParentClass = parent == null ? "" : parent.Name;
				} else {
					// 编辑时
					ParentClass = bindFrom.Parent == null ? "" : bindFrom.Parent.Name;
					// 检查类型，防止越权操作
					if (bindFrom.Type != Type) {
						throw new HttpException(403, new T("Try to access class that type not matched"));
					}
				}
				Name = bindFrom.Name;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, Database.GenericClass saveTo) {
				if (saveTo.Id <= 0) {
					// 添加时
					saveTo.Type = Type;
					saveTo.Parent = GetParentClass(context);
					saveTo.CreateTime = DateTime.UtcNow;
				} else if (saveTo.Type != Type) {
					// 编辑时检查类型，防止越权操作
					throw new HttpException(403, new T("Try to access class that type not matched"));
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
