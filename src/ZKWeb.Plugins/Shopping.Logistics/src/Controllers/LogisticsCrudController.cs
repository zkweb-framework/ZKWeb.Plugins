using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using Newtonsoft.Json;
using ZKWebStandard.Ioc;
using ZKWebStandard.Collection;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
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
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Logistics.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Logistics.src.UIComponents.ListItemProviders;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Controllers {
	using Logistics = Domain.Entities.Logistics;

	/// <summary>
	/// 物流管理
	/// </summary>
	[ExportMany]
	public class LogisticsCrudController : CrudAdminAppControllerBase<Logistics, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "LogisticsManage"; } }
		public override string Url { get { return "/admin/logistics"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-truck"; } }
		protected override IAjaxTableHandler<Logistics, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public LogisticsCrudController() {
			IncludeCss.Add("/static/common.region.css/region-editor.css");
			IncludeCss.Add("/static/shopping.logistics.css/logistics-edit.css");
			IncludeJs.Add("/static/common.region.js/region-editor.min.js");
			IncludeJs.Add("/static/shopping.logistics.js/logistics-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<Logistics, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<LogisticsCrudController>();
				searchBar.StandardSetupFor<LogisticsCrudController>("Name/Remark");
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<Logistics> query) {
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<Logistics>> pairs) {
				foreach (var pair in pairs) {
					var owner = pair.Entity.Owner;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["Type"] = new T(pair.Entity.Type);
					pair.Row["Owner"] = owner == null ? null : owner.Username;
					pair.Row["OwnerId"] = owner == null ? null : (Guid?)owner.Id;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<LogisticsCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddEditColumnFor<UserCrudController>("Owner", "OwnerId");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<LogisticsCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑物流使用的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<Logistics, Guid, Form> {
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(500, MinimumLength = 1)]
			[TextBoxField("Name", "Please enter name")]
			public string Name { get; set; }
			/// <summary>
			/// 类型
			/// </summary>
			[Required]
			[RadioButtonsField("LogisticsType", typeof(LogisticsTypeListItemProvider))]
			public string Type { get; set; }
			/// <summary>
			/// 所属人
			/// </summary>
			[TextBoxField("Owner", "Owner")]
			public string Owner { get; set; }
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
			/// 运费规则的提示信息
			/// </summary>
			[HtmlField("PriceRulesAlert", Group = "LogisticsPriceRules")]
			public HtmlString PriceRulesAlert { get; set; }
			/// <summary>
			/// 运费规则
			/// </summary>
			[LogisticsPriceRulesEditor("PriceRules", Group = "LogisticsPriceRules")]
			public List<PriceRule> PriceRules { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(Logistics bindFrom) {
				Name = bindFrom.Name;
				Type = bindFrom.Type ??
					new LogisticsTypeListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				Owner = bindFrom.Owner == null ? null : bindFrom.Owner.Username;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				PriceRulesAlert = new HtmlString(templateManager.RenderTemplate(
					"shopping.logistics/tmpl.price_rules_alert.html", null));
				PriceRules = bindFrom.PriceRules;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(Logistics saveTo) {
				var userManager = Application.Ioc.Resolve<UserManager>();
				saveTo.Name = Name;
				saveTo.Type = Type;
				saveTo.Owner = Owner == null ? null : userManager.Get(u => u.Username == Owner);
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				saveTo.PriceRules = PriceRules;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
