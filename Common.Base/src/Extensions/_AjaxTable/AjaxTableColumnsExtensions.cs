using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// Ajax表格列的扩展函数
	/// </summary>
	public static class AjaxTableColumnsExtensions {
		/// <summary>
		/// 添加Id列（多选框+批量操作菜单）
		/// 默认添加全选/取消全选的菜单项
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员名称</param>
		/// <param name="width">宽度</param>
		public static AjaxTableIdColumn AddIdColumn(
			this List<AjaxTableColumn> columns, string member, string width = "2%") {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var column = new AjaxTableIdColumn() {
				Key = member,
				Width = width,
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.id_column_cell.html", new { member })
			};
			columns.Add(column);
			column.AddSelectOrUnselectAll();
			return column;
		}

		/// <summary>
		/// 添加序号列（从1开始递增，不涉及到数据）
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="width">宽度</param>
		public static AjaxTableColumn AddNoColumn(
			this List<AjaxTableColumn> columns, string width = "2%") {
			var column = new AjaxTableColumn() {
				Key = "No",
				Width = width,
				HeadTemplate = "",
				CellTemplate = "<%-result.PageNo * result.PageSize + index%>"
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加成员列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="width">宽度</param>
		public static AjaxTableColumn AddMemberColumn(
			this List<AjaxTableColumn> columns, string member, string width = null) {
			var column = new AjaxTableColumn() {
				Key = member,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(member)),
				CellTemplate = string.Format("<%-row.{0}%>", HttpUtils.HtmlEncode(member))
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加Html成员列
		/// 成员值会作为Html嵌入到页面中，请做好安全处理
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="width">宽度</param>
		public static AjaxTableColumn AddHtmlColumn(
			this List<AjaxTableColumn> columns, string member, string width = null) {
			var column = new AjaxTableColumn() {
				Key = member,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(member)),
				CellTemplate = string.Format("<%=row.{0}%>", HttpUtils.HtmlEncode(member))
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加显示枚举值的标签列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="enumType">枚举类型</param>
		/// <param name="width">宽度</param>
		/// <returns></returns>
		public static AjaxTableColumn AddEnumLabelColumn(
			this IList<AjaxTableColumn> columns, string member, Type enumType, string width = null) {
			var enums = enumType.GetEnumValues().OfType<Enum>();
			var classMapping = new HtmlString(JsonConvert.SerializeObject(
				enums.ToDictionary(e => (int)(object)e, e => {
					var attr = e.GetAttribute<LabelCssClassAttribute>();
					return attr == null ? null : attr.CssClass;
				})));
			var nameMapping = new HtmlString(JsonConvert.SerializeObject(
				enums.ToDictionary(e => (int)(object)e, e => new T(e.GetDescription()))));
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var column = new AjaxTableColumn() {
				Key = member,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(member)),
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.label_column_cell.html",
					new { classMapping, nameMapping, member })
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加图片列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="width">宽度</param>
		/// <param name="imageWidth">图片宽度</param>
		/// <param name="imageHeight">图片高度</param>
		/// <returns></returns>
		public static AjaxTableColumn AddImageColumn(
			this IList<AjaxTableColumn> columns, string member,
			string width = "30", string imageWidth = "30", string imageHeight = "30") {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var column = new AjaxTableColumn() {
				Key = member,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(member)),
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.image_column_cell.html",
					new { member, imageWidth, imageHeight })
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加树节点列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="nameMember">名称成员</param>
		/// <param name="levelMember">层次成员，开始值是0</param>
		/// <param name="noChildsMember">判断是否没有子节点的成员</param>
		/// <param name="width">宽度</param>
		/// <returns></returns>
		public static AjaxTableColumn AddTreeNodeColumn(
			this IList<AjaxTableColumn> columns,
			string nameMember, string levelMember, string noChildsMember, string width = null) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var column = new AjaxTableColumn() {
				Key = nameMember,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(nameMember)),
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.tree_node_column_cell.html",
					new { nameMember, levelMember, noChildsMember })
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加点击时执行指定代码的列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="onClick">点击时执行的代码</param>
		/// <param name="width">宽度</param>
		/// <returns></returns>
		public static AjaxTableColumn AddOnClickColumn(
			this List<AjaxTableColumn> columns, string member, string onClick, string width = null) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var column = new AjaxTableColumn() {
				Key = member,
				Width = width,
				HeadTemplate = HttpUtils.HtmlEncode(new T(member)),
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.onclick_cell.html",
					new { member, onClick })
			};
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 添加显示远程内容的模态框的列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="nameMember">名称成员</param>
		/// <param name="titleTemplate">模态框标题的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="urlTemplate">远程链接的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		/// <param name="width">宽度</param>
		/// <returns></returns>
		public static AjaxTableColumn AddRemoteModalColumn(
			this List<AjaxTableColumn> columns, string nameMember,
			string titleTemplate, string urlTemplate,
			object dialogParameters = null, string width = null) {
			var onclick = string.Format(@"
				var table = $(this).closestAjaxTable();
				var row = table.getBelongedRowData(this);
				row && table.showRemoteModalForRow(row, {0}, {1}, {2});",
				JsonConvert.SerializeObject(titleTemplate),
				JsonConvert.SerializeObject(urlTemplate),
				JsonConvert.SerializeObject(dialogParameters));
			return columns.AddOnClickColumn(nameMember, onclick, width);
		}

		/// <summary>
		/// 添加操作列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="width">宽度</param>
		public static AjaxTableActionColumn AddActionColumn(
			this IList<AjaxTableColumn> columns, string width = "5%") {
			var column = new AjaxTableActionColumn() { Width = width };
			columns.Add(column);
			return column;
		}

		/// <summary>
		/// 移动列到指定的列后面
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="column">需要移动的列</param>
		/// <param name="key">指定的列的识别键</param>
		public static void MoveAfter(
			this List<AjaxTableColumn> columns, AjaxTableColumn column, string key) {
			columns.Remove(column);
			columns.AddAfter(c => c.Key == key, column);
		}

		/// <summary>
		/// 移动列到指定的列前面
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="column">需要移动的列</param>
		/// <param name="key">指定的列的识别键</param>
		public static void MoveBefore(
			this List<AjaxTableColumn> columns, AjaxTableColumn column, string key) {
			columns.Remove(column);
			columns.AddBefore(c => c.Key == key, column);
		}
	}
}
