using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Admin Login", "管理員登錄" },
			{ "Please enter username", "請填寫用戶名" },
			{ "Please enter password", "請填寫密碼" },
			{ "Username", "用戶名" },
			{ "Password", "密碼" },
			{ "Login", "登陸" },
			{ "Register new user", "注冊新用戶" },
			{ "RememberLogin", "記住登陸" },
			{ "Register", "注冊" },
			{ "ConfirmPassword", "確認密碼" },
			{ "Please repeat the password exactly", "請重複前面填寫的密碼" },
			{ "OldPassword", "原密碼" },
			{ "Please enter old password", "請填寫原密碼" },
			{ "User Registration", "用戶注冊" },
			{ "User Login", "用戶登錄" },
			{ "Username is already taken, please choose other username", "用戶名已經被使用，請選擇其他用戶名" },
			{ "You have registered successfully, thanks for you registration", "注冊用戶成功，感謝您的注冊" },
			{ "User Panel", "用戶中心" },
			{ "Login successful", "登陸成功" },
			{ "Welcome to ", "歡迎光臨" },
			{ "Logout", "退出登陸" },
			{ "Register for free", "免費注冊" },
			{ "Website has no admin yet, the first login user will become super admin.",
				"當前沒有任何管理員，第一次登錄的用戶將會成為超級管理員" },
			{ "You have already logged in, continue will replace the logged in user.",
				"您已經登陸，繼續登陸將會替換當前登錄的用戶" },
			{ "Sorry, You have no privileges to use admin panel.", "抱歉，你沒有使用管理後台的權限" },
			{ "Incorrect username or password", "用戶名或密碼不正確，請重新填寫" },
			{ "Apps", "應用" },
			{ "Workspace", "工作間" },
			{ "Website Index", "網站首頁" },
			{ "About Me", "關於我" },
			{ "About Website", "關於網站" },
			{ "Admin Panel", "管理後台" },
			{ "My Apps", "我的應用" },
			{ "Action require {0}, and {1} privileges", "操作要求擁有{0}身份，且擁有{1}權限" },
			{ "Action require {0}", "操作要求擁有{0}身份" },
			{ "User", "用戶" },
			{ "UserType", "用戶類型" },
			{ "Admin", "管理員" },
			{ "SuperAdmin", "超級管理員" },
			{ "CooperationPartner", "合作伙伴" },
			{ "CreateTime", "創建時間" },
			{ "Admin Manage", "管理員管理" },
			{ "User Manage", "用戶管理" },
			{ "Role Manage", "角色管理" },
			{ "Role", "角色" },
			{ "Roles", "角色" },
			{ "UserRole", "角色" },
			{ "View", "查看" },
			{ "Edit", "編輯" },
			{ "Delete", "刪除" },
			{ "DeleteForever", "永久刪除" },
			{ "Please enter name", "請填寫名稱" },
			{ "Remark", "備注" },
			{ "Please enter remark", "請填寫備注" },
			{ "Saved Successfully", "保存成功" },
			{ "Keep empty if you don't want to change", "如果不想修改，請保持空白" },
			{ "Name/Remark", "名稱/備注" },
			{ "Name", "名稱" },
			{ "Value", "值" },
			{ "DirectoryName", "目錄名稱" },
			{ "Description", "描述" },
			{ "LastUpdated", "更新時間" },
			{ "Add {0}", "添加{0}" },
			{ "Edit {0}", "編輯{0}" },
			{ "Delete {0}", "刪除{0}" },
			{ "Please enter password when creating admin", "創建管理員時需要填寫密碼" },
			{ "Please enter password when creating user", "創建用戶時需要填寫密碼" },
			{ "You can't downgrade yourself to normal admin", "不能取消自身的超級管理員權限" },
			{ "Privileges", "權限" },
			{ "Recycle Bin", "回收站" },
			{ "Batch Delete", "批量刪除" },
			{ "Please select {0} to delete", "請選擇需要刪除的{0}" },
			{ "Sure to delete following {0}?", "確認刪除以下{0}？" },
			{ "Batch Recover", "批量恢復" },
			{ "Please select {0} to recover", "請選擇需要恢復的{0}" },
			{ "Sure to recover following {0}?", "確認恢復以下{0}？" },
			{ "Batch Delete Forever", "批量永久刪除" },
			{ "Sure to delete following {0} forever?", "確認永久刪除以下{0}？此操作將不可恢復！" },
			{ "Delete yourself is not allowed", "不能刪除你自身的用戶" },
			{ "Action {0} not exist", "找不到{0}對應的操作" },
			{ "Delete Successful", "刪除成功" },
			{ "Batch Delete Successful", "批量刪除成功" },
			{ "Batch Recover Successful", "批量恢復成功" },
			{ "Batch Delete Forever Successful", "批量永久刪除成功" },
			{ "Change Password", "修改密碼" },
			{ "Change Avatar", "修改頭像" },
			{ "Avatar", "頭像" },
			{ "DeleteAvatar", "刪除頭像" },
			{ "Please select avatar file", "請選擇頭像圖片" },
			{ "Parse uploaded image failed", "解析上傳的圖片失敗" },
			{ "User not found", "用戶不存在" },
			{ "Incorrect old password", "原密碼不正確，請重新填寫" },
			{ "No Role", "無角色" },
			{ "Website Name", "網站名稱" },
			{ "Default Language", "默認語言" },
			{ "Default Timezone", "默認時區" },
			{ "Hosting Information", "服務器信息" },
			{ "Plugin List", "插件列表" },
			{ "Admin panel and users management", "提供管理後台和用戶角色管理等功能" },
			{ "Clear Cache", "清理緩存" },
			{ "Clear Cache Successfully", "清理緩存成功" },
			{ "Server Username", "服務器用戶" },
			{ "Version", "版本" },
			{ "FullVersion", "完整版本" },
			{ "ZKWeb Version", "ZKWeb版本" },
			{ "ZKWeb Full Version", "ZKWeb完整版本" },
			{ "Memory Usage", "使用内存" },
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
