using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldValidators {
	[ExportMany(ContractKey = typeof(StringLengthAttribute)), SingletonReuse]
	public class StringLength {
	}
}
