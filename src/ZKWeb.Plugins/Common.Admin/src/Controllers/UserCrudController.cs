using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
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
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 用户管理
	/// </summary>
	[ExportMany]
	public class UserCrudController : CrudAdminAppControllerBase<User, Guid> {
		public override string Group { get { return "System"; } }
		public override string GroupIconClass { get { return "fa fa-gear"; } }
		public override string Name { get { return "User Manage"; } }
		public override string Url { get { return "/admin/users"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-user"; } }
		protected override IAjaxTableHandler<User, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new AddForm(); }
		protected override IModelFormBuilder GetEditForm() { return new EditForm(); }

		/// <summary>
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<User, Guid> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<UserCrudController>();
				searchBar.StandardSetupFor<UserCrudController>("Username");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<User> query) {
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
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Avatar"] = userManager.GetAvatarWebPath(pair.Entity.Id);
					pair.Row["Username"] = pair.Entity.Username;
					pair.Row["UserType"] = new T(pair.Entity.Type);
					pair.Row["Roles"] = string.Join(", ", pair.Entity.Roles.Select(r => r.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<UserCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddImageColumn("Avatar");
				response.Columns.AddMemberColumn("Username", "45%");
				response.Columns.AddMemberColumn("UserType");
				response.Columns.AddMemberColumn("Roles");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<UserCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑共用的基础表单
		/// </summary>
		public abstract class BaseForm : TabEntityFormBuilder<User, Guid, BaseForm> {
			/// <summary>
			/// 密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("Password", "Keep empty if you don't want to change", Group = "Change Password")]
			public string Password { get; set; }
			/// <summary>
			/// 确认密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("ConfirmPassword", "Keep empty if you don't want to change", Group = "Change Password")]
			public string ConfirmPassword { get; set; }
			/// <summary>
			/// 头像
			/// </summary>
			[FileUploaderField("Avatar", Group = "Change Avatar")]
			public IHttpPostedFile Avatar { get; set; }
			/// <summary>
			/// 删除头像
			/// </summary>
			[CheckBoxField("DeleteAvatar", Group = "Change Avatar")]
			public bool DeleteAvatar { get; set; }

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(User saveTo) {
				// 添加时要求填密码，并设置用户类型是"用户"
				if (saveTo.Id == Guid.Empty) {
					if (string.IsNullOrEmpty(Password)) {
						throw new BadRequestException(new T("Please enter password when creating user"));
					}
					saveTo.Type = NormalUserType.ConstType;
				}
				// 添加用户或修改密码需要超级管理员
				if (!string.IsNullOrEmpty(Password)) {
					var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
					privilegeManager.Check(typeof(IAmSuperAdmin));
					if (Password != ConfirmPassword) {
						throw new BadRequestException(new T("Please repeat the password exactly"));
					}
					saveTo.SetPassword(Password);
				}
				return this.SaveSuccessAndCloseModal();
			}

			/// <summary>
			/// 保存头像
			/// </summary>
			protected override void OnSubmitSaved(User saved) {
				var userManagr = Application.Ioc.Resolve<UserManager>();
				if (Avatar != null) {
					userManagr.SaveAvatar(saved.Id, Avatar.OpenReadStream());
				} else if (DeleteAvatar) {
					userManagr.DeleteAvatar(saved.Id);
				}
			}
		}

		/// <summary>
		/// 添加表单，允许设置用户名
		/// </summary>
		public class AddForm : BaseForm {
			[Required]
			[StringLength(100, MinimumLength = 3)]
			[TextBoxField("Username", "Please enter username")]
			public string Username { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(User bindFrom) { }

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(User saveTo) {
				saveTo.Username = Username;
				return base.OnSubmit(saveTo);
			}
		}

		/// <summary>
		/// 编辑表单，用户名只读
		/// </summary>
		public class EditForm : BaseForm {
			[LabelField("Username")]
			public string Username { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(User bindFrom) {
				Username = bindFrom.Username;
			}
		}
	}
}
