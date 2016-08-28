using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms {
	/// <summary>
	/// 用于编辑实体的表单
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
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class EntityFormBuilder<TEntity, TPrimaryKey, TForm> :
		ModelFormBuilder
		where TEntity : class, IEntity<TPrimaryKey>, new() {
		/// <summary>
		/// 扩展列表
		/// </summary>
		protected IList<IEntityFormExtraHandler<TEntity, TPrimaryKey, TForm>> ExtraHandlers { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public EntityFormBuilder(FormBuilder form = null) : base(form) {
			ExtraHandlers = Application.Ioc.ResolveMany<
				IEntityFormExtraHandler<TEntity, TPrimaryKey, TForm>>().ToList();
			ExtraHandlers.ForEach(c => c.OnCreated((TForm)(object)this));
		}

		/// <summary>
		/// 绑定实体到表单
		/// </summary>
		/// <param name="bindFrom">来源的实体</param>
		protected abstract void OnBind(TEntity bindFrom);

		/// <summary>
		/// 保存表单到实体，返回处理结果
		/// </summary
		/// <param name="saveTo">保存到的实体</param>
		/// <returns></returns>
		protected abstract object OnSubmit(TEntity saveTo);

		/// <summary>
		/// 实体保存后的处理，用于添加关联实体等
		/// </summary>
		/// <param name="saved">已保存的实体，Id已分配</param>
		protected virtual void OnSubmitSaved(TEntity saved) { }

		/// <summary>
		/// 获取传入参数请求的实体Id
		/// </summary>
		/// <returns></returns>
		protected virtual TPrimaryKey GetRequestId() {
			var request = HttpManager.CurrentContext.Request;
			var id = request.Get<string>("id") ?? request.Get<string>("Id");
			return id.ConvertOrDefault<TPrimaryKey>();
		}

		/// <summary>
		/// 绑定时的处理
		/// 支持通过扩展修改表单
		/// </summary>
		protected sealed override void OnBind() {
			var id = GetRequestId();
			var uow = Application.Ioc.Resolve<IUnitOfWork>();
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			using (uow.Scope()) {
				var entity = (id.EqualsSupportsNull(default(TPrimaryKey))) ?
					new TEntity() : service.Get(id);
				if (entity == null) {
					throw new NotFoundException(
						string.Format(new T("Data with id {0} cannot be found"), id));
				}
				OnBind(entity);
				ExtraHandlers.ForEach(c => c.OnBind((TForm)(object)this, entity));
			}
		}

		/// <summary>
		/// 提交时的处理，返回处理结果
		/// </summary>
		/// <returns></returns>
		protected sealed override object OnSubmit() {
			var id = GetRequestId();
			var uow = Application.Ioc.Resolve<IUnitOfWork>();
			var service = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			using (uow.Scope()) {
				var data = (id.EqualsSupportsNull(default(TPrimaryKey))) ?
					new TEntity() : service.Get(id);
				if (data == null) {
					throw new NotFoundException(
						string.Format(new T("Data with id {0} cannot be found"), id));
				}
				// 保存时的处理
				object formResult = null;
				service.Save(ref data, d => {
					formResult = OnSubmit(d);
					ExtraHandlers.ForEach(c => c.OnSubmit((TForm)(object)this, d));
				});
				// 保存后的处理
				OnSubmitSaved(data);
				ExtraHandlers.ForEach(c => c.OnSubmitSaved((TForm)(object)this, data));
				return formResult;
			}
		}
	}
}
