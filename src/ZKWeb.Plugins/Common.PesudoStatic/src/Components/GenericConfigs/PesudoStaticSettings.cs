using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs.Attributes;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.PesudoStatic.Enums;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Components.GenericConfigs {
	/// <summary>
	/// 伪静态设置
	/// </summary>
	[GenericConfig("Common.PesudoStatic.PesudoStatisSettings", CacheTime = 15)]
	public class PesudoStaticSettings {
		/// <summary>
		/// 启用伪静态
		/// 默认启用
		/// </summary>
		public bool EnablePesudoStatic { get; set; }
		/// <summary>
		/// 伪静态扩展名
		/// 默认".html"
		/// </summary>
		public string PesudoStaticExtension { get; set; }
		/// <summary>
		/// 伪静态参数分隔符
		/// 默认"."
		/// </summary>
		public char PesudoStaticParamDelimiter { get; set; }
		/// <summary>
		/// 伪静态策略
		/// 默认黑名单策略
		/// </summary>
		public PesudoStaticPolicies PesudoStaticPolicy { get; set; }
		/// <summary>
		/// 包含的Url路径
		/// 在白名单策略下生效
		/// </summary>
		public SortedSet<string> IncludeUrlPaths { get; set; }
		/// <summary>
		/// 排除的Url路径
		/// 在黑名单策略下生效
		/// </summary>
		public SortedSet<string> ExcludeUrlPaths { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PesudoStaticSettings() {
			EnablePesudoStatic = true;
			PesudoStaticExtension = ".html";
			PesudoStaticParamDelimiter = '.';
			PesudoStaticPolicy = PesudoStaticPolicies.BlackListPolicy;
			IncludeUrlPaths = new SortedSet<string>();
			ExcludeUrlPaths = new SortedSet<string>();
		}
	}
}
