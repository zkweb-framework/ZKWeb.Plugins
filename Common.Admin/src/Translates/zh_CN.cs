using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Admin Login", "管理员登陆" },
			{ "Please enter username", "请填写用户名" },
			{ "Please enter password", "请填写密码" },
			{ "Username", "用户名" },
			{ "Password", "密码" },
			{ "Login", "登陆" },
			{ "Register new user", "注册新用户" },
			{ "RememberLogin", "记住登陆" },
			{ "Register", "注册" },
			{ "ConfirmPassword", "确认密码" },
			{ "Please repeat the password exactly", "请重复前面填写的密码" },
			{ "User Registration", "用户注册" },
			{ "User Login", "用户登录" },
			{ "Username is already taken, please choose other username", "用户名已经被使用，请选择其他用户名" },
			{ "You have registered successfully, thanks for you registration", "注册用户成功，感谢您的注册" },
			{ "User Panel", "用户中心" },
			{ "Login successful", "登陆成功" },
			{ "Welcome to ", "欢迎光临" },
			{ "Logout", "退出登陆" },
			{ "Register for free", "免费注册" },
			{ "Website has no admin yet, the first login user will become super admin.",
				"当前没有任何管理员，第一次登录的用户将会成为超级管理员" },
			{ "You have already logged in, continue will replace the logged in user.",
				"您已经登陆，继续登陆将会替换当前登录的用户" },
			{ "Sorry, You have no privileges to use admin panel.", "抱歉，你没有使用管理后台的权限" },
			{ "Incorrect username or password", "用户名或密码不正确，请重新填写" },
			{ "Apps", "应用" },
			{ "Workspace", "工作间" },
			{ "Website Index", "网站首页" },
			{ "About Me", "关于我" },
			{ "About Website", "关于网站" },
			{ "Admin Panel", "管理后台" },
			{ "My Apps", "我的应用" },
			{ "Access this page require {0}, and {1} privileges", "访问此页面要求拥有身份{0}，且拥有权限{1}" },
			{ "Access this page require {0}", "访问此页面要求拥有身份{0}" },
			{ "User", "用户" },
			{ "Admin", "管理员" },
			{ "SuperAdmin", "超级管理员" },
			{ "CooperationPartner", "合作伙伴" },
			{ "CreateTime", "创建时间" },
			{ "Admin Manage", "管理员管理" },
			{ "User Manage", "用户管理" },
			{ "Role Manage", "角色管理" },
			{ "Role", "角色" },
			{ "View", "查看" },
			{ "Please enter name", "请填写名称" },
			{ "Remark", "备注" },
			{ "Please enter remark", "请填写备注" },
			{ "Successfully Saved", "保存成功" },
			{ "Keep empty if you don't want to change", "如果不想修改，请保持空白" },
			{ "Name/Remark", "名称/备注" },
			{ "Name", "名称" },
			{ "LastUpdated", "更新时间" },
			{ "Add User", "添加用户" },
			{ "Edit User", "编辑用户" },
			{ "Add UserRole", "添加角色" },
			{ "Edit UserRole", "编辑角色" },
			{ "Add Admin", "添加管理员" },
			{ "Edit Admin", "编辑管理员" },
			{ "Please enter password when creating admin", "创建管理员时需要填写密码" },
			{ "You can't downgrade yourself to normal admin", "不能取消自身的超级管理员权限" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
