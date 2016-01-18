using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "ZKWeb Default Website", "ZKWeb默认站点" },
			{ "Captcha", "验证码" },
			{ "Click to change captcha image", "点击更换验证码图片" },
			{ "Please enter captcha", "请填写验证码" },
			{ "Incorrect captcha", "验证码错误，请重新填写" },
			{ "{0} is required", "请填写{0}" },
			{ "Length of {0} must be {1}", "{0}的长度必须是{1}" },
			{ "Length of {0} must between {1} and {2}", "{0}的长度必须在{1}和{2}之间" },
			{ "HomePage", "首页" },
			{ "How to edit this page", "怎样编辑这个页面" },
			{ "Use Plugin", "使用插件" },
			{ "Copy Common.Base/templates/common.base/index.html to Your.Plugin/templates/common.base/index.html then edit it.",
				"复制Common.Base/templates/common.base/index.html到你的插件/templates/common.base/index.html然后编辑" },
			{ "Use Diy", "使用Diy" },
			{ "Diy is not ready yet.", "Diy功能尚未完成" },
			{ "Refresh", "刷新" },
			{ "Fullscreen", "全屏" },
			{ "Operations", "操作" },
			{ "Export to excel", "导出到表格" },
			{ "Print", "打印" },
			{ "Pagination Settings", "分页设置" },
			{ "[0] Records per page", "每页[0]条" },
			{ "Please enter keyword", "请填写关键词" },
			{ "Advance search", "高级搜索" },
			{ "Data with id {0} cannot be found", "无法找到Id是{0}的数据" },
			{ "True", "是" },
			{ "False", "否" },
			{ "Ok", "确认" },
			{ "Cancel", "取消" },
			{ "Actions", "操作" },
			{ "Deleted", "已删除" },
			{ "Select All", "全选" },
			{ "Select/Unselect All", "全选/取消全选" },
			{ "Submit", "提交" },
			{ "Please Select", "请选择" },
			{ "Only {0} files are allowed", "只允许上传{0}文件" },
			{ "Please upload file size not greater than {0}", "请上传大小不超过{0}的文件" },
			{ "Basic Information", "基本信息" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
