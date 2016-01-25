using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.AdminSettings.src;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWeb.Plugins.Common.MenuPageBase.src;

namespace ZKWeb.Plugins.Common.GenericTag.src {
	/// <summary>
	/// 通用标签构建器
	/// 使用时需要继承，例子
	/// [ExportMany]
	/// public class ExampleTag : GenericTagBuilder {
	///		public override string Name { get { return "ExampleTag"; } }
	/// }
	/// </summary>
	public abstract class GenericTagBuilder :
		GenericListForAdminSettings<Database.GenericTag, GenericTagBuilder> {
		/// <summary>
		/// 标签类型，默认使用名称（除去空格）
		/// </summary>
		public virtual string Type { get { return Name.Replace(" ", ""); } }
		/// <summary>
		/// 使用的权限
		/// </summary>
		public override string Privilege { get { return "TagManage:" + Type; } }
		/// <summary>
		/// 所属分组
		/// </summary>
		public override string Group { get { return "TagManage"; } }
		/// <summary>
		/// 分组图标
		/// </summary>
		public override string GroupIcon { get { return "fa fa-tags"; } }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public override string IconClass { get { return "fa fa-tags"; } }
		/// <summary>
		/// Url地址
		/// </summary>
		public override string Url { get { return "/admin/settings/generic_tag/" + Type.ToLower(); } }
		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected override IAjaxTableCallback<Database.GenericTag> GetTableCallback() { return new TableCallback(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.GenericTag> {
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				throw new NotImplementedException();
			}

			public void OnQuery(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericTag> query) {
				throw new NotImplementedException();
			}

			public void OnSort(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.GenericTag> query) {
				throw new NotImplementedException();
			}

			public void OnSelect(AjaxTableSearchRequest request, List<KeyValuePair<Database.GenericTag, Dictionary<string, object>>> pairs) {
				throw new NotImplementedException();
			}

			public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				throw new NotImplementedException();
			}
		}
	}
}
