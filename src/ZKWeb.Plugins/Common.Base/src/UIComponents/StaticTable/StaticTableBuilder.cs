using DotLiquid;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using System.Linq;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable {
	/// <summary>
	/// 静态表格构建器
	/// </summary>
	public class StaticTableBuilder : ILiquidizable {
		/// <summary>
		/// 表格的Css类
		/// </summary>
		public string TableClass { get; set; }
		/// <summary>
		/// 表格头部单元格的Css类
		/// </summary>
		public string TableHeadRowClass { get; set; }
		/// <summary>
		/// 列列表
		/// </summary>
		public IList<Column> Columns { get; set; }
		/// <summary>
		/// 数据列表
		/// </summary>
		public IList<object> Rows { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public StaticTableBuilder() {
			TableClass = "table table-bordered table-hover";
			TableHeadRowClass = "heading";
			Columns = new List<Column>();
			Rows = new List<object>();
		}

		/// <summary>
		/// 获取转换成Hash对象的数据列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Hash> GetHashRows() {
			foreach (var row in Rows) {
				var dict = row as IDictionary<string, object>;
				if (dict != null)
					yield return Hash.FromDictionary(dict);
				else
					yield return Hash.FromAnonymousObject(dict);
			}
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public object ToLiquid() {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var result = templateManager.RenderTemplate(
				"common.base/tmpl.static_table.html", new {
					tableClass = TableClass,
					tableHeadRowClass = TableHeadRowClass,
					columns = Columns,
					rows = GetHashRows()
				});
			return new HtmlString(result);
		}

		/// <summary>
		/// 列类型
		/// </summary>
		public class Column : ILiquidizable {
			/// <summary>
			/// 成员
			/// </summary>
			public string Member { get; set; }
			/// <summary>
			/// 宽度
			/// </summary>
			public string Width { get; set; }
			/// <summary>
			/// 是否允许标题和成员使用Html
			/// </summary>
			public bool AllowHtml { get; set; }
			/// <summary>
			/// 标题，默认使用成员名称
			/// </summary>
			public string Caption { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="member">成员</param>
			/// <param name="width">宽度</param>
			/// <param name="allowHtml">是否允许标题和成员使用Html，默认不允许</param>
			/// <param name="caption">标题，默认使用成员名称</param>
			public Column(string member, string width = null,
				bool allowHtml = false, string caption = null) {
				Member = member;
				Width = width;
				AllowHtml = allowHtml;
				Caption = caption ?? new T(member);
			}

			/// <summary>
			/// 允许在模板中使用
			/// </summary>
			/// <returns></returns>
			public object ToLiquid() {
				return new {
					member = Member,
					width = Width,
					allowHtml = AllowHtml,
					caption = Caption
				};
			}
		}
	}
}
