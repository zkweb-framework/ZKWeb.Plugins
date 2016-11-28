using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Structs;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Domain.Services {
	/// <summary>
	/// 通用记录管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericRecordManager : DomainServiceBase<Entities.GenericRecord, Guid> {
		/// <summary>
		/// 添加纪录
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <param name="releatedId">关联数据Id，可以等于null</param>
		/// <param name="creatorId">创建人用户Id，可以等于null</param>
		/// <param name="content">记录内容</param>
		/// <param name="keepTime">保留时间，等于null时永久保留</param>
		/// <param name="extraData">附加数据</param>
		public virtual void AddRecord(
			string type, Guid? releatedId, Guid? creatorId,
			string content, TimeSpan? keepTime = null, object extraData = null) {
			var userService = Application.Ioc.Resolve<IDomainService<User, Guid>>();
			var now = DateTime.UtcNow;
			var record = new Entities.GenericRecord() {
				Type = type,
				ReleatedId = releatedId,
				Creator = (creatorId == null) ? null : userService.Get(creatorId.Value),
				KeepUntil = (keepTime == null) ? null : (DateTime?)now.Add(keepTime.Value),
				Content = content,
				ExtraData = extraData.ConvertOrDefault<GenericRecordExtraData>()
			};
			Save(ref record);
		}

		/// <summary>
		/// 根据类型查找记录
		/// 默认按Id倒序排列
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <returns></returns>
		public virtual IList<Entities.GenericRecord> FindRecords(string type) {
			return GetMany(query => query
				.Where(r => r.Type == type)
				.OrderByDescending(r => r.Id).ToList());
		}

		/// <summary>
		/// 根据类型和关联数据Id查找记录
		/// 默认按Id倒序排列
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <param name="releatedId">关联数据Id</param>
		/// <returns></returns>
		public virtual IList<Entities.GenericRecord> FindRecords(string type, Guid? releatedId) {
			return GetMany(query => query
				.Where(r => r.Type == type && r.ReleatedId == releatedId)
				.OrderByDescending(r => r.CreateTime).ToList());
		}

		/// <summary>
		/// 清理过期的记录
		/// </summary>
		/// <returns></returns>
		public virtual long ClearExpiredRecords() {
			var now = DateTime.UtcNow;
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(p => p.KeepUntil != null && p.KeepUntil < now);
			}
		}
	}
}
