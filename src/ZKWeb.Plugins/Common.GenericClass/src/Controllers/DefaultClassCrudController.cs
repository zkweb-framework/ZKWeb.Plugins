using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.GenericClasses {
	/// <summary>
	/// 默认分类的控制器
	/// </summary>
	[ExportMany]
	public class DefaultClassCrudController : CrudGenericClassControllerBase {
		public override string Name { get { return "DefaultClass"; } }
	}
}
