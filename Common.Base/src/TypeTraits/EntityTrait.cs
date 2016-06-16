using System;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using System.FastReflection;
using System.Collections.Concurrent;

namespace ZKWeb.Plugins.Common.Base.src.TypeTraits {
	/// <summary>
	/// 实体类型的特征类
	/// </summary>
	public class EntityTrait {
		/// <summary>
		/// 主键
		/// </summary>
		public string PrimaryKey { get; set; }
		/// <summary>
		/// 主键类型
		/// </summary>
		public Type PrimaryKeyType { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public EntityTrait() { }

		/// <summary>
		/// 初始化
		/// </summary>
		public EntityTrait(Type type) {
			var idMember = type.FastGetProperty("Id");
			if (idMember != null) {
				PrimaryKey = "Id";
				PrimaryKeyType = idMember.PropertyType;
			}
		}

		/// <summary>
		/// 默认特征类的缓存
		/// </summary>
		private static ConcurrentDictionary<Type, EntityTrait> DefaultTraits =
			new ConcurrentDictionary<Type, EntityTrait>();

		/// <summary>
		/// 获取指定实体类型的特征
		/// </summary>
		/// <param name="type">实体类型</param>
		/// <returns></returns>
		public static EntityTrait For(Type type) {
			var trait = Application.Ioc.Resolve<EntityTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			if (trait == null) {
				trait = DefaultTraits.GetOrAdd(type, t => new EntityTrait(t));
			}
			return trait;
		}

		/// <summary>
		/// 获取指定实体类型的特征
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		/// <returns></returns>
		public static EntityTrait For<T>() {
			return For(typeof(T));
		}

		/// <summary>
		/// 用于保存获取主键的函数的类
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		private class PrimaryKeyGetters<T> {
			public readonly static Lazy<Func<T, object>> Func =
				new Lazy<Func<T, object>>(() => ReflectionUtils.MakeGetter<T, object>(For<T>().PrimaryKey));
		}

		/// <summary>
		/// 获取实体的主键
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		/// <param name="data">实体对象</param>
		/// <returns></returns>
		public static object GetPrimaryKey<T>(T data) {
			return PrimaryKeyGetters<T>.Func.Value(data);
		}
	}
}
