using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities {
	/// <summary>
	/// 自定义翻译
	/// </summary>
	[ExportMany]
	public class CustomTranslation :
		IEntity<Guid>, IEntityMappingProvider<CustomTranslation> {
		/// <summary>
		/// 翻译Id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 语言
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// 原文
		/// </summary>
		public string Original { get; set; }
		/// <summary>
		/// 译文
		/// </summary>
		public string Translated { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public void Configure(IEntityMappingBuilder<CustomTranslation> builder) {
			builder.Id(t => t.Id);
			builder.Map(t => t.Language, new EntityMappingOptions() { Index = "Idx_Language" });
			builder.Map(t => t.Original, new EntityMappingOptions() { Index = "Idx_Original" });
			builder.Map(t => t.Translated);
		}

		/// <summary>
		/// 显示原文
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Original;
		}
	}
}
