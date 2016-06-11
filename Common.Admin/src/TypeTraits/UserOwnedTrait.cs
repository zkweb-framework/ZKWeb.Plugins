using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Admin.src.TypeTraits {
	/// <summary>
	/// 类型是否有所属用户的特征类
	/// </summary>
	public class UserOwnedTrait {
		/// <summary>
		/// 默认的所属用户成员名称列表
		/// </summary>
		private static readonly string[] DefaultPropertyNames = new string[] { "Owner", "User" };
		/// <summary>
		/// 所属用户的成员名称
		/// </summary>
		public string PropertyName { get; set; }
		/// <summary>
		/// 是否有所属用户
		/// </summary>
		public bool IsUserOwned { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public UserOwnedTrait() { }

		/// <summary>
		/// 根据类型初始化
		/// </summary>
		/// <param name="type">实体类型</param>
		public UserOwnedTrait(Type type) {
			foreach (var ownerKey in DefaultPropertyNames) {
				if (type.FastGetProperty(ownerKey) != null) {
					PropertyName = ownerKey;
					IsUserOwned = true;
					break;
				}
			}
		}

		/// <summary>
		/// 默认特征类的缓存
		/// </summary>
		private static ConcurrentDictionary<Type, UserOwnedTrait> DefaultTraits =
			new ConcurrentDictionary<Type, UserOwnedTrait>();

		/// <summary>
		/// 返回指定类型是否有所属用户的特征
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public static UserOwnedTrait For(Type type) {
			var trait = Application.Ioc.Resolve<UserOwnedTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			if (trait == null) {
				trait = DefaultTraits.GetOrAdd(type, t => new UserOwnedTrait(t));
			}
			return trait;
		}

		/// <summary>
		/// 返回指定类型是否有所属用户的特征
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		public static UserOwnedTrait For<T>() {
			return For(typeof(T));
		}
	}
}
