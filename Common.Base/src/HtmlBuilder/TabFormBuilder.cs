using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.HtmlBuilder {
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
		protected virtual void RenderTabContainerBeginTag(HtmlTextWriter html) {
			html.AddAttribute("class", "tabbable-line");
			html.RenderBeginTag("div");
		}

		/// <summary>
		/// 描画标签容器的结束标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderTabContainerEndTag(HtmlTextWriter html) {
			html.RenderEndTag(); // div
		}

		/// <summary>
		/// 描画标签
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="groups">字段分组</param>
		protected virtual void RenderTabs(
			HtmlTextWriter html, IEnumerable<IGrouping<string, FormField>> groups) {
			html.AddAttribute("class", "nav nav-tabs");
			html.RenderBeginTag("ul");
			// 描画标签
			var firstGroup = true;
			foreach (var group in groups) {
				if (firstGroup) {
					html.AddAttribute("class", "active");
					firstGroup = false;
				}
				html.RenderBeginTag("li");
				html.AddAttribute("href", "#" + GetTabId(group.Key));
				html.AddAttribute("data-toggle", "tab");
				html.AddAttribute("aria-expanded", "true");
				html.RenderBeginTag("a");
				html.WriteEncodedText(new T(group.Key));
				html.RenderEndTag(); // a
				html.RenderEndTag(); // li
			}
			// 描画提交按钮
			html.AddAttribute("class", "buttons");
			html.RenderBeginTag("li");
			RenderSubmitButton(html);
			html.RenderEndTag(); // li
			html.RenderEndTag(); // ul
		}

		/// <summary>
		/// 描画标签内容
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="groups">字段分组</param>
		protected virtual void RenderTabContents(
			HtmlTextWriter html, IEnumerable<IGrouping<string, FormField>> groups) {
			html.AddAttribute("class", "tab-content");
			html.RenderBeginTag("div");
			var firstGroup = true;
			foreach (var group in groups) {
				html.AddAttribute("id", GetTabId(group.Key));
				html.AddAttribute("class", firstGroup ? "tab-pane active" : "tab-pane");
				firstGroup = false;
				html.RenderBeginTag("div");
				foreach (var field in group) {
					RenderFormField(html, field);
				}
				html.RenderEndTag(); // div
			}
			html.RenderEndTag(); // div
		}

		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var html = new HtmlTextWriter(new StringWriter());
			RenderFormBeginTag(html);
			RenderTabContainerBeginTag(html);
			var groups = Fields.GroupBy(f => f.Attribute.Group ?? DefaultGroupName).ToList();
			RenderTabs(html, groups);
			RenderTabContents(html, groups);
			RenderTabContainerEndTag(html);
			RenderCsrfToken(html);
			RenderFormEndTag(html);
			return html.InnerWriter.ToString();
		}
	}
}
