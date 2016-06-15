using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Currency.src.Model {
	/// <summary>
	/// 货币的接口
	/// </summary>
	public interface ICurrency {
		/// <summary>
		/// 货币类型
		/// 格式是3位英文大写（ISO 4217）
		/// 储存时请以这个成员为准
		/// </summary>
		string Type { get; }
		/// <summary>
		/// 货币的前缀
		/// </summary>
		string Prefix { get; }
		/// <summary>
		/// 货币的后缀
		/// </summary>
		string Suffix { get; }
	}

	/// <summary>
	/// 货币的扩展函数
	/// </summary>
	public static class ICurrencyExtensions {
		/// <summary>
		/// 金额保留的小数位
		/// </summary>
		public static int RoundDecimals = 2;

		/// <summary>
		/// 格式化金额字符串
		/// 前缀 + 金额 + 后缀
		/// </summary>
		/// <param name="currency">货币</param>
		/// <param name="amount">金额</param>
		/// <returns></returns>
		public static string Format(this ICurrency currency, decimal amount) {
			amount = Math.Round(amount, RoundDecimals);
			return string.Format("{0}{1}{2}", currency.Prefix, amount, currency.Suffix);
		}
	}
}
