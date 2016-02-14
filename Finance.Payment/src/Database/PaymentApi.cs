using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.Database {
	/// <summary>
	/// 支付接口
	/// 用于用户支付金钱给网站或其他用户
	/// </summary>
	public class PaymentApi {
		/// <summary>
		/// 接口Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 接口类型
		/// 创建后不能修改
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 接口的所属用户，null时属于网站
		/// 创建后不能修改
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual Dictionary<string, object> ExtraData { get; set; }
		/// <summary>
		/// 支持的交易类型列表
		/// </summary>
		public virtual List<string> SupportTransactionTypes { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApi() {
			DisplayOrder = 10000;
			ExtraData = new Dictionary<string, object>();
			SupportTransactionTypes = new List<string>();
		}

		/// <summary>
		/// 返回支付接口名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}
	}

	/// <summary>
	/// 支付接口的数据库结构
	/// </summary>
	[ExportMany]
	public class PaymentApiMap : ClassMap<PaymentApi> {
		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApiMap() {
			Id(a => a.Id);
			Map(a => a.Name);
			Map(a => a.Type).Index("Idx_Type");
			References(a => a.Owner).Nullable();
			Map(a => a.ExtraData).CustomType<JsonSerializedType<Dictionary<string, object>>>();
			Map(a => a.SupportTransactionTypes).CustomType<JsonSerializedType<List<string>>>();
			Map(a => a.CreateTime);
			Map(a => a.LastUpdated);
			Map(a => a.Deleted);
			Map(a => a.DisplayOrder);
			Map(a => a.Remark).Length(0xffff);
		}
	}
}
