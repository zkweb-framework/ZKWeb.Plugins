using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Repositories {
	/// <summary>
	/// 通用记录的数据仓储
	/// </summary>
	[ExportMany]
	public class GenericRecordRepository : GenericRepository<Database.GenericRecord> {
		/// <summary>
		/// 添加纪录
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <param name="releatedId">关联数据Id，可以等于null</param>
		/// <param name="creatorId">创建人用户Id，可以等于null</param>
		/// <param name="content">记录内容</param>
		/// <param name="keepTime">保留时间，等于null时永久保留</param>
		/// <param name="extraData">附加数据</param>
		public void AddRecord(
			string type, long? releatedId, long? creatorId,
			string content, TimeSpan? keepTime = null, object extraData = null) {
			var now = DateTime.UtcNow;
			var record = new Database.GenericRecord() {
				Type = type,
				ReleatedId = releatedId,
				Creator = Context.Get<User>(u => u.Id == creatorId),
				CreateTime = now,
				KeepUntil = (keepTime == null) ? null : (DateTime?)now.Add(keepTime.Value),
				Content = content,
				ExtraData = extraData.ConvertOrDefault<Dictionary<string, object>>()
			};
			Context.Save(ref record);
		}

		/// <summary>
		/// 根据类型查找记录
		/// 默认按Id倒序排列
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <returns></returns>
		public IQueryable<Database.GenericRecord> FindRecords(string type) {
			return Context.Query<Database.GenericRecord>()
				.Where(r => r.Type == type).OrderByDescending(r => r.Id);
		}

		/// <summary>
		/// 根据类型和关联数据Id查找记录
		/// 默认按Id倒序排列
		/// </summary>
		/// <param name="type">记录类型</param>
		/// <param name="releatedId">关联数据Id</param>
		/// <returns></returns>
		public IQueryable<Database.GenericRecord> FindRecords(string type, long? releatedId) {
			return Context.Query<Database.GenericRecord>()
				.Where(r => r.Type == type && r.ReleatedId == releatedId)
				.OrderByDescending(r => r.Id);
		}
	}
}
