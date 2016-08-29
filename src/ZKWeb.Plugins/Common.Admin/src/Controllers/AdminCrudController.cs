using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Extensions;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 管理员管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class AdminCrudController : CrudAdminAppControllerBase<User, Guid> {
		public override string Name { get { return "Admin Manage"; } }
		public override string Url { get { return "/admin/admins"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-user-secret"; } }
		public override string EntityTypeName { get { return "Admin"; } }
		public override Type RequiredUserType { get { return typeof(IAmSuperAdmin); } }
		protected override IAjaxTableHandler<User, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new AddForm(); }
		protected override IModelFormBuilder GetEditForm() { return new EditForm(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<User, Guid> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public override void BuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<AdminCrudController>();
				searchBar.StandardSetupFor<AdminCrudController>("Username");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<User> query) {
				var adminTypes = Application.Ioc.ResolveMany<IUserType>()
					.Where(t => t is IAmAdmin).Select(t => t.Type).ToList();
				query = query.Where(u => adminTypes.Contains(u.Type));
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(u => u.Username.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<User>> pairs) {
				var userManager = Application.Ioc.Resolve<UserManager>();
				foreach (var pair in pairs) {
					var userType = pair.Entity.GetUserType();
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Avatar"] = userManager.GetAvatarWebPath(pair.Entity.Id);
					pair.Row["Username"] = pair.Entity.Username;
					pair.Row["Roles"] = string.Join(", ", pair.Entity.Roles.Select(r => r.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["SuperAdmin"] = (
						userType is IAmSuperAdmin ? EnumBool.True : EnumBool.False);
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<AdminCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddImageColumn("Avatar");
				response.Columns.AddMemberColumn("Username", "45%");
				response.Columns.AddMemberColumn("Roles");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddEnumLabelColumn("SuperAdmin", typeof(EnumBool));
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<AdminCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑共用的编辑表单
		/// </summary>
		public class BaseForm : TabEntityFormBuilder<User, Guid, BaseForm> {
			/// <summary>
			/// 密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("Password", "Keep empty if you don't want to change")]
			public string Password { get; set; }
			/// <summary>
			/// 确认密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("ConfirmPassword", "Keep empty if you don't want to change")]
			public string ConfirmPassword { get; set; }
			/// <summary>
			/// 是否超级管理员
			/// </summary>
			[CheckBoxField("SuperAdmin")]
			public bool IsSuperAdmin { get; set; }
			/// <summary>
			/// 角色
			/// </summary>
			[CheckBoxGroupField("Roles", typeof(ListItemFromEntities<UserRole, Guid>))]
			public HashSet<Guid> Roles { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(User bindFrom) {
				Password = null;
				IsSuperAdmin = bindFrom.GetUserType() is IAmSuperAdmin;
				Roles = new HashSet<Guid>(bindFrom.Roles.Select(r => r.Id));
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(User saveTo) {
				// 添加时要求填密码
				if (saveTo.Id == Guid.Empty && string.IsNullOrEmpty(Password)) {
					throw new BadRequestException(new T("Please enter password when creating admin"));
				}
				// 需要更新密码时
				if (!string.IsNullOrEmpty(Password)) {
					if (Password != ConfirmPassword) {
						throw new BadRequestException(new T("Please repeat the password exactly"));
					}
					saveTo.SetPassword(Password);
				}
				// 选中超级管理员时设置超级管理员
				saveTo.Type = IsSuperAdmin ? SuperAdminUserType.ConstType : AdminUserType.ConstType;
				// 不允许取消自身的超级管理员权限
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				if (sessionManager.GetSession().ReleatedId == saveTo.Id &&
					saveTo.Type != SuperAdminUserType.ConstType) {
					throw new BadRequestException(new T("You can't downgrade yourself to normal admin"));
				}
				// 设置角色
				var roleService = Application.Ioc.Resolve<IDomainService<UserRole, Guid>>();
				saveTo.Roles = new HashSet<UserRole>(roleService.GetMany(r => Roles.Contains(r.Id)));
				return this.SaveSuccessAndCloseModal();
			}
		}

		/// <summary>
		/// 添加表单
		/// </summary>
		public class AddForm : BaseForm {
			/// <summary>
			/// 用户名
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 3)]
			[TextBoxField("Username", "Please enter username")]
			public string Username { get; set; }

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(User saveTo) {
				saveTo.Username = Username;
				return base.OnSubmit(saveTo);
			}
		}

		/// <summary>
		/// 编辑表单
		/// </summary>
		public class EditForm : BaseForm {
			/// <summary>
			/// 用户名
			/// </summary>
			[LabelField("Username")]
			public string Username { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(User bindFrom) {
				Username = bindFrom.Username;
				base.OnBind(bindFrom);
			}
		}
	}
}
