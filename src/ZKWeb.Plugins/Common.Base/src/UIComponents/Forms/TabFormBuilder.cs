using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms {
	/// <summary>
	/// 表单构建器，支持多标签
	/// 按字段的Group分组，没有指定Group的字段分到"基本信息"中
	/// </summary>
	public class TabFormBuilder : FormBuilder {
		/// <summary>
		/// 默认分组名称
		/// </summary>
		public const string DefaultGroupName = "Basic Information";

		/// <summary>
		/// 从分组名称获取标签Id，不带#
		/// </summary>
		/// <param name="groupName">分组名称</param>
		/// <returns></returns>
		protected virtual string GetTabId(string groupName) {
			return "Tab" + groupName.Replace(" ", "");
		}

		/// <summary>
		/// 描画标签容器的开始标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderTabContainerBeginTag(StringBuilder html) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.tab_form.begin_tag.html", null));
		}

		/// <summary>
		/// 描画标签容器的结束标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderTabContainerEndTag(StringBuilder html) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.tab_form.end_tag.html", null));
		}

		/// <summary>
		/// 描画标签
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="groups">字段分组</param>
		protected virtual void RenderTabs(
			StringBuilder html, IEnumerable<IGrouping<string, FormField>> groups) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var submitButton = new StringBuilder();
			RenderSubmitButton(submitButton);
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.tab_form.tabs.html", new {
					groups = groups.Select(g => new { tabId = GetTabId(g.Key), name = new T(g.Key) }),
					submitButton = new HtmlString(submitButton.ToString())
				}));
		}

		/// <summary>
		/// 描画标签内容
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="groups">字段分组</param>
		protected virtual void RenderTabContents(
			StringBuilder html, IEnumerable<IGrouping<string, FormField>> groups) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.tab_form.tab_contents.html", new {
					groups = groups.Select(g => new {
						tabId = GetTabId(g.Key),
						formFields = g.Select(field => {
							var formFieldHtml = new StringBuilder();
							RenderFormField(formFieldHtml, field);
							return new HtmlString(formFieldHtml.ToString());
						}).ToList()
					}).ToList()
				}));
		}

		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var html = new StringBuilder();
			RenderFormBeginTag(html);
			RenderTabContainerBeginTag(html);
			var groups = Fields.GroupBy(f => f.Attribute.Group ?? DefaultGroupName).ToList();
			RenderTabs(html, groups);
			RenderTabContents(html, groups);
			RenderTabContainerEndTag(html);
			RenderCsrfToken(html);
			RenderFormEndTag(html);
			return html.ToString();
		}
	}
}
