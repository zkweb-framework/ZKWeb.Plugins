using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Components.ListItemProviders;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;

namespace ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms {
	/// <summary>
	/// 编辑支付接口使用的表单
	/// </summary>
	public class PaymentApiEditForm : TabEntityFormBuilder<PaymentApi, Guid, PaymentApiEditForm> {
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 1)]
		[TextBoxField("Name", "Please enter name")]
		public string Name { get; set; }
		/// <summary>
		/// 所属人
		/// </summary>
		[TextBoxField("Owner", "Please enter owner's username, keep empty if owned by system")]
		public string Owner { get; set; }
		/// <summary>
		/// 支持的交易类型
		/// </summary>
		[Required]
		[CheckBoxGroupField("SupportTransactionTypes", typeof(PaymentTransactionTypeListItemProvider))]
		public List<string> SupportTransactionTypes { get; set; }
		/// <summary>
		/// 显示顺序
		/// </summary>
		[Required]
		[TextBoxField("DisplayOrder", "Order from small to large")]
		public long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[TextAreaField("Remark", 5, "Remark", Group = "Remark")]
		public string Remark { get; set; }
		/// <summary>
		/// 使用的支付接口处理器列表
		/// </summary>
		public IList<IPaymentApiHandler> Handlers { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApiEditForm() {
			var id = GetRequestId();
			var apiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var api = id == Guid.Empty ?
				new PaymentApi() { Type = Request.Get<string>("type") } :
				apiManager.Get(id);
			Handlers = api.GetHandlers();
			Handlers.ForEach(h => h.OnFormCreated(this));
		}

		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		protected override void OnBind(PaymentApi bindFrom) {
			// 获取接口类型
			var type = bindFrom.Type ?? Request.Get<string>("type");
			if (string.IsNullOrEmpty(type)) {
				throw new ArgumentException(new T("Payment api type not specificed"));
			}
			// 绑定表单
			Name = bindFrom.Name;
			Owner = bindFrom.Owner == null ? null : bindFrom.Owner.Username;
			SupportTransactionTypes = bindFrom.SupportTransactionTypes;
			DisplayOrder = bindFrom.DisplayOrder;
			Remark = bindFrom.Remark;
			// 调用支付接口处理器的绑定表单事件
			Handlers.ForEach(h => h.OnFormBind(this, bindFrom));
		}

		/// <summary>
		/// 保存表单到数据
		/// </summary>
		protected override object OnSubmit(PaymentApi saveTo) {
			// 添加接口时设置类型
			if (string.IsNullOrEmpty(saveTo.Type)) {
				saveTo.Type = Request.Get<string>("type");
			}
			// 保存表单
			var userManager = Application.Ioc.Resolve<UserManager>();
			saveTo.Name = Name;
			saveTo.Owner = string.IsNullOrEmpty(Owner) ?
				null : userManager.Get(u => u.Username == Owner);
			saveTo.SupportTransactionTypes = SupportTransactionTypes;
			saveTo.UpdateTime = DateTime.UtcNow;
			saveTo.DisplayOrder = DisplayOrder;
			saveTo.Remark = Remark;
			if (!string.IsNullOrEmpty(Owner) && saveTo.Owner == null) {
				throw new ArgumentException(new T("Specificed payment api owner not exist"));
			} else if (string.IsNullOrEmpty(saveTo.Type)) {
				throw new ArgumentException(new T("Payment api type not specificed"));
			}
			// 调用支付接口处理器的保存表单事件
			Handlers.ForEach(h => h.OnFormSubmit(this, saveTo));
			return this.SaveSuccessAndCloseModal();
		}
	}
}
