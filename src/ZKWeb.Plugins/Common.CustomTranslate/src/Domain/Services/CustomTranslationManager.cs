using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Services {
	/// <summary>
	/// 自定义翻译管理器
	/// </summary>
	[ExportMany]
	public class CustomTranslationManager : DomainServiceBase<CustomTranslation, Guid> {
		/// <summary>
		/// 根据语言获取所有翻译
		/// </summary>
		/// <param name="language">语言</param>
		/// <returns></returns>
		public virtual IDictionary<string, string> GetManyForLanguage(string language) {
			var translations = GetMany(t => t.Language == language);
			var result = new Dictionary<string, string>();
			translations.ForEach(t => result[t.Original] = t.Translated);
			return result;
		}
	}
}
