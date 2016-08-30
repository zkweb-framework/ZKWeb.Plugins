using ZKWeb.Plugins.Common.GenericClass.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.Controllers {
	/// <summary>
	/// 默认分类的控制器
	/// </summary>
	[ExportMany]
	public class DefaultClassController : GenericClassControllerBase {
		public override string Name { get { return "DefaultClass"; } }
	}
}
