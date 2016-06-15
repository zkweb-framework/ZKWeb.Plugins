using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 菜单项列表的分组
	/// </summary>
	public class MenuItemGroup : ILiquidizable {
		/// <summary>
		/// 分组名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public string IconClass { get; set; }
		/// <summary>
		/// 菜单项列表
		/// </summary>
		public List<MenuItem> Items { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">分组名称</param>
		/// <param name="iconClass">图标的Css类</param>
		public MenuItemGroup(string name, string iconClass) {
			Name = name;
			IconClass = iconClass;
			Items = new List<MenuItem>();
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { Name, IconClass, Items };
		}
	}
}
