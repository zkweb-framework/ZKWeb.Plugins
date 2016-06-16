using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// 用于编辑指定数据的表单
	/// </summary>
	/// <example>
	/// public class UserEditForm : DataEditFormBuilder[User, UserEditForm] {
	///		[TextBoxField("Username")]
	///		public string Username { get; set; }
	///		protected override void OnBind(User bindFrom) {
	///			Username = bindFrom.Username;
	///		}
	///		protected override object OnSubmit(User saveTo) {
	///			saveTo.Username = Username;
	///			return new { message = new T("Saved Successfully") };
	///		}
	/// }
	/// </example>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class DataEditFormBuilder<TData, TForm> : ModelFormBuilder
		where TData : class, new() {
		/// <summary>
		/// 扩展列表
		/// </summary>
		protected List<IDataEditFormExtension<TData, TForm>> Extensions { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DataEditFormBuilder(FormBuilder form = null) : base(form) {
			Extensions = Application.Ioc.ResolveMany<IDataEditFormExtension<TData, TForm>>().ToList();
			Extensions.ForEach(c => c.OnCreated((TForm)(object)this));
		}

		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="bindFrom">来源的数据</param>
		protected abstract void OnBind(DatabaseContext context, TData bindFrom);

		/// <summary>
		/// 保存表单到数据，返回处理结果
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="saveTo">保存到的数据</param>
		/// <returns></returns>
		protected abstract object OnSubmit(DatabaseContext context, TData saveTo);

		/// <summary>
		/// 数据保存后的处理，用于添加关联数据等
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="saved">已保存的数据，Id已分配</param>
		protected virtual void OnSubmitSaved(DatabaseContext context, TData saved) { }

		/// <summary>
		/// 获取传入参数请求的数据Id
		/// </summary>
		/// <returns></returns>
		protected virtual string GetRequestId() {
			var request = HttpManager.CurrentContext.Request;
			var id = request.Get<string>("id") ?? request.Get<string>("Id");
			return id;
		}

		/// <summary>
		/// 绑定时的处理
		/// 支持通过扩展修改表单
		/// </summary>
		protected sealed override void OnBind() {
			var id = GetRequestId();
			UnitOfWork.ReadData<TData>(r => {
				var data = string.IsNullOrEmpty(id) ? new TData() : r.GetById(id);
				if (data == null) {
					throw new HttpException(404, string.Format(new T("Data with id {0} cannot be found"), id));
				}
				OnBind(r.Context, data);
				Extensions.ForEach(c => c.OnBind((TForm)(object)this, r.Context, data));
			});
		}

		/// <summary>
		/// 提交时的处理，返回处理结果
		/// 支持通过扩展保存数据，支持使用IDataSaveCallback检测数据的修改
		/// </summary>
		/// <returns></returns>
		protected sealed override object OnSubmit() {
			var id = GetRequestId();
			var result = UnitOfWork.WriteData<TData, object>(r => {
				var data = string.IsNullOrEmpty(id) ? new TData() : r.GetById(id);
				if (data == null) {
					throw new HttpException(404, string.Format(new T("Data with id {0} cannot be found"), id));
				}
				// 保存时的处理
				object formResult = null;
				r.Save(ref data, d => {
					formResult = OnSubmit(r.Context, d);
					Extensions.ForEach(c => c.OnSubmit((TForm)(object)this, r.Context, d));
				});
				// 保存后的处理
				OnSubmitSaved(r.Context, data);
				Extensions.ForEach(c => c.OnSubmitSaved((TForm)(object)this, r.Context, data));
				return formResult;
			});
			return result;
		}
	}
}
