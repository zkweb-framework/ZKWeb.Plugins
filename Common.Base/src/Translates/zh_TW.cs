using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Translates {
	/// <summary>
	/// 繁体中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_TW : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-TW" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ZKWeb Default Website", "ZKWeb默認站點" },
			{ "Captcha", "驗證碼" },
			{ "Click to change captcha image", "點擊更換驗證碼圖片" },
			{ "Please enter captcha", "請填寫驗證碼" },
			{ "Incorrect captcha", "驗證碼錯誤，請重新填寫" },
			{ "{0} is required", "請填寫{0}" },
			{ "Length of {0} must be {1}", "{0}的長度必須是{1}" },
			{ "Length of {0} must between {1} and {2}", "{0}的長度必須在{1}和{2}之間" },
			{ "HomePage", "首頁" },
			{ "Index", "首頁" },
			{ "How to edit this page", "怎樣編輯這個頁面" },
			{ "Use Plugin", "使用插件" },
			{ "Copy Common.Base/templates/common.base/index.html to Your.Plugin/templates/common.base/index.html then edit it.",
				"復制Common.Base/templates/common.base/index.html到你的插件/templates/common.base/index.html然後編輯" },
			{ "Use Diy", "使用Diy" },
			{ "Diy is not ready yet.", "Diy功能尚未完成" },
			{ "Refresh", "刷新" },
			{ "Fullscreen", "全屏" },
			{ "Operations", "操作" },
			{ "Export to excel", "導出到表格" },
			{ "Print", "打印" },
			{ "Pagination Settings", "分頁設置" },
			{ "[0] Records per page", "每頁[0]條" },
			{ "Please enter keyword", "請填寫關鍵詞" },
			{ "Search", "搜索" },
			{ "AdvanceSearch", "高級搜索" },
			{ "Data with id {0} cannot be found", "無法找到Id是{0}的數據" },
			{ "True", "是" },
			{ "False", "否" },
			{ "Yes", "是" },
			{ "No", "否" },
			{ "Ok", "確認" },
			{ "Cancel", "取消" },
			{ "Actions", "操作" },
			{ "Deleted", "已刪除" },
			{ "Select All", "全選" },
			{ "Select/Unselect All", "全選/取消全選" },
			{ "Submit", "提交" },
			{ "Please Select", "請選擇" },
			{ "Only {0} files are allowed", "只允許上傳{0}文件" },
			{ "Please upload file size not greater than {0}", "請上傳大小不超過{0}的文件" },
			{ "Basic Information", "基本信息" },
			{ "Base Functions", "基礎功能" },
			{ "Base functions and template pages", "基礎的功能和模板頁面" },
			{ "{0} format is incorrect", "{0}的格式不正確" },
			{ "Expand/Collapse All", "展開/折疊全部" },
			{ "Type", "類型" },
			{ "Menu", "菜單" },
			{ "BatchActions", "批量操作" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
