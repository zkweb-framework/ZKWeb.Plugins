using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.AdminSettings.src;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.CustomTranslate.src.Model;

namespace ZKWeb.Plugins.Common.CustomTranslate.src {
	/// <summary>
	/// 自定义翻译器
	/// 可以在后台设置中设置自定义翻译
	/// 例子
	///		...
	/// </summary>
	public abstract class CustomTranslator :
		GenericListForAdminSettings<TranslateContent, CustomTranslator> {
		/// <summary>
		/// 使用的权限
		/// </summary>
		public override string Privilege { get { return "CustomTranslate:" + Name; } }
		/// <summary>
		/// 所属分组
		/// </summary>
		public override string Group { get { return "CustomTranslate"; } }
		/// <summary>
		/// 分组图标
		/// </summary>
		public override string GroupIcon { get { return "fa fa-language"; } }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public override string IconClass { get { return "fa fa-language"; } }
		/// <summary>
		/// Url地址
		/// </summary>
		public override string Url { get { return "/admin/settings/custom_translate/" + Name.ToLower(); } }
		/// <summary>
		/// 添加使用的Url地址
		/// </summary>
		public virtual string AddUrl { get { return Url + "/add"; } }
		/// <summary>
		/// 编辑使用的Url地址
		/// </summary>
		public virtual string EditUrl { get { return Url + "/edit"; } }
		/// <summary>
		/// 删除使用的Url地址
		/// </summary>
		public virtual string DeleteUrl { get { return Url + "/delete"; } }

		/// <summary>
		/// 获取表格回调
		/// </summary>
		/// <returns></returns>
		protected override IAjaxTableCallback<TranslateContent> GetTableCallback() {
			return new TableCallback(Name, AddUrl, EditUrl, DeleteUrl);
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<TranslateContent> {
			/// <summary>
			/// 语言名称
			/// </summary>
			public string Name { get; set; }
			/// <summary>
			/// 添加使用的Url地址
			/// </summary>
			public string AddUrl { get; set; }
			/// <summary>
			/// 编辑使用的Url地址
			/// </summary>
			public string EditUrl { get; set; }
			/// <summary>
			/// 删除使用的Url地址
			/// </summary>
			public string DeleteUrl { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public TableCallback(string name, string addUrl, string editUrl, string deleteUrl) {
				Name = name;
				AddUrl = addUrl;
				EditUrl = editUrl;
				DeleteUrl = deleteUrl;
			}

			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<TranslateContent> query) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<TranslateContent> query) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<TranslateContent, Dictionary<string, object>>> pairs) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				throw new NotImplementedException();
			}
		}
	}
}
