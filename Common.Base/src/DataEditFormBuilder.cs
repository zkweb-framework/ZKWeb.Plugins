using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 用于编辑指定数据的表单
	/// 例子
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
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class DataEditFormBuilder<TData, TForm> : ModelFormBuilder
		where TData : class, new() {
		/// <summary>
		/// 回调列表
		/// </summary>
		protected List<IDataEditFormCallback<TData, TForm>> Callbacks { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DataEditFormBuilder(FormBuilder form = null) : base(form) {
			Callbacks = Application.Ioc.ResolveMany<IDataEditFormCallback<TData, TForm>>().ToList();
			Callbacks.ForEach(c => c.OnCreated((TForm)(object)this));
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
			var request = HttpContext.Current.Request;
			var id = request.GetParam<string>("id") ?? request.GetParam<string>("Id");
			return id;
		}

		/// <summary>
		/// 绑定时的处理
		/// 支持通过回调修改表单
		/// </summary>
		protected sealed override void OnBind() {
			var id = GetRequestId();
			GenericRepository.UnitOfWork<TData>(repository => {
				var data = string.IsNullOrEmpty(id) ? new TData() : repository.GetById(id);
				if (data == null) {
					throw new HttpException(404, string.Format(new T("Data with id {0} cannot be found"), id));
				}
				OnBind(repository.Context, data);
				Callbacks.ForEach(c => c.OnBind((TForm)(object)this, repository.Context, data));
			});
		}

		/// <summary>
		/// 提交时的处理，返回处理结果
		/// 支持通过回调保存数据，支持使用IDataSaveCallback检测数据的修改
		/// </summary>
		/// <returns></returns>
		protected sealed override object OnSubmit() {
			var id = GetRequestId();
			object result = null;
			GenericRepository.UnitOfWorkMayChangeData<TData>(repository => {
				var data = string.IsNullOrEmpty(id) ? new TData() : repository.GetById(id);
				if (data == null) {
					throw new HttpException(404, string.Format(new T("Data with id {0} cannot be found"), id));
				}
				// 保存时的处理
				repository.Save(ref data, d => {
					result = OnSubmit(repository.Context, d);
					Callbacks.ForEach(c => c.OnSubmit((TForm)(object)this, repository.Context, d));
				});
				// 保存后的处理
				OnSubmitSaved(repository.Context, data);
				Callbacks.ForEach(c => c.OnSubmitSaved((TForm)(object)this, repository.Context, data));
			});
			return result;
		}
	}
}
