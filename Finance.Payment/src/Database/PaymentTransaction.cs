using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Database.UserTypes;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Database {
	/// <summary>
	/// 支付交易
	/// 用于储存单笔交易的信息
	/// </summary>
	public class PaymentTransaction {
		/// <summary>
		/// 交易Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 交易流水号，唯一键
		/// </summary>
		public virtual string Serial { get; set; }
		/// <summary>
		/// 交易类型
		/// 创建后不能修改
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 使用的支付接口
		/// </summary>
		public virtual PaymentApi Api { get; set; }
		/// <summary>
		/// 外部提供的交易流水号
		/// 例如支付宝上的交易号
		/// </summary>
		public virtual string ExternalSerial { get; set; }
		/// <summary>
		/// 金额
		/// </summary>
		public virtual decimal Amount { get; set; }
		/// <summary>
		/// 支付手续费
		/// 手续费有可能在网站上收取，也可能在支付接口上收取
		/// </summary>
		public virtual decimal PaymentFee { get; set; }
		/// <summary>
		/// 货币类型
		/// </summary>
		public virtual string CurrencyType { get; set; }
		/// <summary>
		/// 付款人，可以等于null
		/// </summary>
		public virtual User Payer { get; set; }
		/// <summary>
		/// 收款人，可以等于null
		/// 如果支付接口有所属人，这里应该等于该所属人
		/// </summary>
		public virtual User Payee { get; set; }
		/// <summary>
		/// 关联数据Id
		/// 根据交易类型不同关联的数据类型也不同
		/// </summary>
		public virtual long? ReleatedId { get; set; }
		/// <summary>
		/// 关联的交易列表
		/// 可以用于合并交易等有关联的交易类型
		/// </summary>
		public virtual ISet<PaymentTransaction> ReleatedTransactions { get; set; }
		/// <summary>
		/// 交易描述
		/// </summary>
		public virtual string Description { get; set; }
		/// <summary>
		/// 交易状态
		/// </summary>
		public virtual PaymentTransactionState State { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual PaymentTransactionExtraData ExtraData { get; set; }
		/// <summary>
		/// 最后发生的错误
		/// 没有发生错误时等于空
		/// </summary>
		public virtual string LastError { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentTransaction() {
			ReleatedTransactions = new HashSet<PaymentTransaction>();
			ExtraData = new PaymentTransactionExtraData();
		}

		/// <summary>
		/// 返回流水号
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Serial;
		}
	}

	/// <summary>
	/// 支付交易的数据库结构
	/// </summary>
	[ExportMany]
	public class PaymentTransactionMap : ClassMap<PaymentTransaction> {
		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentTransactionMap() {
			Id(t => t.Id);
			Map(t => t.Serial).Not.Nullable().Unique();
			Map(t => t.Type).Index("Idx_Type");
			References(t => t.Api).Not.Nullable();
			Map(t => t.ExternalSerial).Index("Idx_ExternalSerial");
			Map(t => t.Amount).Index("Idx_Amount");
			Map(t => t.PaymentFee).Index("Idx_PaymentFee");
			Map(t => t.CurrencyType).Index("Idx_CurrencyType");
			References(t => t.Payer).Nullable();
			References(t => t.Payee).Nullable();
			Map(t => t.ReleatedId).Index("Idx_ReleatedId");
			HasMany(t => t.ReleatedTransactions);
			Map(t => t.Description).Length(0xffff);
			Map(t => t.State).Index("Idx_State");
			Map(t => t.ExtraData).CustomType<JsonSerializedType<PaymentTransactionExtraData>>();
			Map(t => t.LastError).Length(0xffff);
			Map(t => t.CreateTime);
			Map(t => t.LastUpdated);
			Map(t => t.Deleted);
			Map(t => t.Remark).Length(0xffff);
		}
	}
}
