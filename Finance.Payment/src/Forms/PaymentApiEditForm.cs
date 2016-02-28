using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.ListItemProviders;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.Forms {
	/// <summary>
	/// 编辑支付接口使用的表单
	/// </summary>
	public class PaymentApiEditForm : TabDataEditFormBuilder<PaymentApi, PaymentApiEditForm> {
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
		public List<IPaymentApiHandler> Handlers { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApiEditForm() {
			var id = GetRequestId();
			UnitOfWork.ReadData<PaymentApi>(repository => {
				var api = string.IsNullOrEmpty(id) ? null : repository.GetById(id);
				var type = api == null ? null : api.Type;
				type = type ?? HttpContext.Current.Request.GetParam<string>("type");
				Handlers = Application.Ioc.ResolvePaymentApiHandlers(type);
			});
			Handlers.ForEach(h => h.OnFormCreated(this));
		}

		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		protected override void OnBind(DatabaseContext context, PaymentApi bindFrom) {
			// 获取接口类型
			var type = bindFrom.Type ?? HttpContext.Current.Request.GetParam<string>("type");
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
			Handlers.ForEach(h => h.OnFormBind(this, context, bindFrom));
		}

		/// <summary>
		/// 保存表单到数据
		/// </summary>
		protected override object OnSubmit(DatabaseContext context, PaymentApi saveTo) {
			// 添加接口时设置类型和创建时间
			if (saveTo.Id <= 0) {
				saveTo.Type = HttpContext.Current.Request.GetParam<string>("type");
				saveTo.CreateTime = DateTime.UtcNow;
			}
			// 保存表单
			saveTo.Name = Name;
			saveTo.Owner = string.IsNullOrEmpty(Owner) ? null : context.Get<User>(u => u.Username == Owner);
			saveTo.SupportTransactionTypes = SupportTransactionTypes;
			saveTo.LastUpdated = DateTime.UtcNow;
			saveTo.DisplayOrder = DisplayOrder;
			saveTo.Remark = Remark;
			if (!string.IsNullOrEmpty(Owner) && saveTo.Owner == null) {
				throw new ArgumentException(new T("Specificed payment api owner not exist"));
			} else if (string.IsNullOrEmpty(saveTo.Type)) {
				throw new ArgumentException(new T("Payment api type not specificed"));
			}
			// 调用支付接口处理器的保存表单事件
			Handlers.ForEach(h => h.OnFormSubmit(this, context, saveTo));
			return new {
				message = new T("Saved Successfully"),
				script = ScriptStrings.AjaxtableUpdatedAndCloseModal
			};
		}
	}
}
