using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.GenericClasses {
	/// <summary>
	/// 默认分类
	/// </summary>
	[ExportMany]
	public class DefaultClass : GenericClassBuilder {
		public override string Name { get { return "DefaultClass"; } }
	}
}
