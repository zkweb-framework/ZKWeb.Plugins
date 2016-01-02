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
			{ "{0} is required", "请填写{0}" },
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
			{ "Register for free", "免费注册" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
