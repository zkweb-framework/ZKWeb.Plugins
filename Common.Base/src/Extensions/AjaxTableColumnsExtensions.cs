using DryIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

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
			// column.AddSelectOrUnselectAll();
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
				CellTemplate = "<%-result.PageIndex * result.PageSize + index + 1%>"
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
				HeadTemplate = HttpUtility.HtmlEncode(new T(member)),
				CellTemplate = string.Format("<%-row.{0}%>", HttpUtility.HtmlAttributeEncode(member))
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
				HeadTemplate = HttpUtility.HtmlEncode(new T(member)),
				CellTemplate = templateManager.RenderTemplate(
					"common.base/tmpl.ajax_table.label_column_cell.html",
					new { classMapping, nameMapping, member })
			};
			columns.Add(column);
			return column;
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
