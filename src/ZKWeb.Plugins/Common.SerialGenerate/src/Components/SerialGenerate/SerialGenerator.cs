using System;
using ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerateHandlers.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerate {
	/// <summary>
	/// 序列号生成器
	/// </summary>
	public static class SerialGenerator {
		/// <summary>
		/// 给指定的数据生成序列号
		/// 默认序列号是 年月日+8位随机数字，共16位
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		public static string GenerateFor(object data) {
			var now = DateTime.UtcNow.ToLocalTime();
			var serial = now.ToString("yyyyMMdd") + RandomUtils.RandomString(8, "0123456789");
			var callbacks = Application.Ioc.ResolveMany<ISerialGenerateHandler>();
			callbacks.ForEach(c => c.OnGenerate(data, ref serial));
			return serial;
		}
	}
}
