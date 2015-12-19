using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	[ExportMany(ContractKey = typeof(DropdownListFieldAttribute)), SingletonReuse]
	public class DropdownList {
	}
}
