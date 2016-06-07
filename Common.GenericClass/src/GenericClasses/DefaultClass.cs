using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.GenericClass.src.GenericClasses {
	/// <summary>
	/// 默认分类
	/// </summary>
	[ExportMany]
	public class DefaultClass : GenericClassBuilder {
		public override string Name { get { return "DefaultClass"; } }
	}
}
