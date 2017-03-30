using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Currency.src.UIComponents.TemplateFilters {
	/// <summary>
	/// 货币使用的模板过滤器
	/// </summary>
	public static class CurrencyFilters {
		/// <summary>
		/// 使用默认货币格式化
		/// </summary>
		/// <param name="amount">金额</param>
		/// <returns></returns>
		/// <example>{{ amount | default_currency }}</example>
		public static string DefaultCurrency(object amount) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var currency = currencyManager.GetDefaultCurrency();
			return currency.Format(amount.ConvertOrDefault<decimal>());
		}

		/// <summary>
		/// 使用指定货币格式化
		/// </summary>
		/// <param name="amount">金额</param>
		/// <param name="currencyType">货币</param>
		/// <returns></returns>
		/// <example>{{ amount | currency: "CNY" }}</example>
		public static string Currency(object amount, string currencyType) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var currency = currencyManager.GetCurrency(currencyType);
			if (currency == null) {
				throw new ArgumentException(
					new T("Currency {0} not found", currencyType));
			}
			return currency.Format(amount.ConvertOrDefault<decimal>());
		}
	}
}
