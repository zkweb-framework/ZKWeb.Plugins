using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src.Components.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "VisualThemeEditor", "可视化编辑器" },
			{ "VisualEditor", "可视化编辑" },
			{ "Allow edit website theme visually", "允许可视化编辑网站主题" },
			{ "AddElement", "添加元素" },
			{ "ManageTheme", "管理主题" },
			{ "SwitchPage", "切换页面" },
			{ "SaveChanges", "保存修改" },
			{ "Please click the page link you want to switch to", "请点击您想切换到的页面的链接" },
			{ "Make sure you have saved all the changes, otherwise they will be lost.",
				"请确认您已保存所有更改, 否则这些更改将会丢失." },
			{ "EnterVisualEditor", "进入可视化编辑" },
			{ "NoDescription", "无描述" },
			{ "RemoveElement", "删除元素" },
			{ "Are you sure to remove $element?", "确认删除$element?" },
			{ "Add Element Success", "添加元素成功" },
			{ "Remove Element Success", "删除元素成功" },
			{ "Edit Element Success", "编辑元素成功" },
			// TODO: 翻译到其他语言
			{ "This widget didn't have any arguments, you can click directly to submit",
				"这个模块无参数, 你可以直接点击提交" },
			{ "AdditionalStyle", "附加样式" },
			{ "__InlineCss", "内嵌CSS" },
			{ "__BeforeHtml", "前置HTML" },
			{ "__AfterHtml", "后置HTML" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
