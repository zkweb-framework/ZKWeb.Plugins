using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 订单发货表单
	/// </summary>
	public class OrderDeliveryForm : EntityFormBuilder<SellerOrder, Guid, OrderDeliveryForm> {
		/// <summary>
		/// 物流配送
		/// </summary>
		[Required]
		[DropdownListField("Logistics", typeof(ListItemFromEntities<Logistics, Guid>))]
		public Guid Logistics { get; set; }
		/// <summary>
		/// 快递单编号
		/// </summary>
		[Required]
		[TextBoxField("LogisticsSerial", PlaceHolder = "LogisticsSerial")]
		public string LogisticsSerial { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[TextBoxField("Remark", PlaceHolder = "Remark")]
		public string Remark { get; set; }
		/// <summary>
		/// 发货件数 { 订单商品Id, 发货数量 }
		/// </summary>
		[Required]
		[JsonField("DeliveryCounts", typeof(IDictionary<Guid, long>))]
		public IDictionary<Guid, long> DeliveryCountsJson { get; set; }
		/// <summary>
		/// 发货商品的表格
		/// </summary>
		[HtmlField("DeliveryProductTable")]
		public HtmlString DeliveryProductTable { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(SellerOrder bindFrom) {
			DeliveryProductTable = new HtmlString("asdasdas");
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(SellerOrder saveTo) {
			throw new NotImplementedException();
		}
	}
}