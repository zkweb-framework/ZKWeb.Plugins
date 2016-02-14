using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Forms;

namespace ZKWeb.Plugins.Finance.Payment.src.Model {
	/// <summary>
	/// 支付接口处理器的接口
	/// 同一接口类型可以注册多个处理器，按注册顺序调用
	/// </summary>
	public interface IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		string Type { get; }

		/// <summary>
		/// 后台编辑表单创建后的处理
		/// </summary>
		/// <param name="form">表单</param>
		void OnFormCreated(PaymentApiEditForm form);

		/// <summary>
		/// 后台编辑表单绑定时的处理
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="bindFrom">支付接口</param>
		void OnFormBind(PaymentApiEditForm form, DatabaseContext context, PaymentApi bindFrom);

		/// <summary>
		/// 后台编辑表单保存时的处理
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="saveTo">支付接口</param>
		void OnFormSubmit(PaymentApiEditForm form, DatabaseContext context, PaymentApi saveTo);

		/// <summary>
		/// 获取支付使用的Html
		/// 是否可以支付已在外部判断，这个函数不需要判断
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <param name="html">支付使用的Html</param>
		/// <returns></returns>
		void GetPaymentHtml(PaymentTransaction transaction, ref HtmlString html);

		/// <summary>
		/// 担保交易时对交易进行发货操作
		/// 发货失败时可以抛出例外阻止事物提交
		/// 同一交易有可能会调用多次，是否可以发货已在外部判断，这个函数不需要判断
		/// </summary>
		/// <param name="transaction">支付交易</param>
		/// <param name="logisticsName">快递或物流名称</param>
		/// <param name="invoiceNo">发货单号</param>
		void SendGoods(PaymentTransaction transaction, string logisticsName, string invoiceNo);
	}
}
