using DryIoc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.Functions;

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
		public EntityTrait() {
			PrimaryKey = "Id";
			PrimaryKeyType = typeof(long);
		}

		/// <summary>
		/// 获取指定实体类型的特征
		/// </summary>
		/// <param name="type">实体类型</param>
		/// <returns></returns>
		public static EntityTrait For(Type type) {
			var trait = Application.Ioc.Resolve<EntityTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			return trait ?? new EntityTrait();
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
