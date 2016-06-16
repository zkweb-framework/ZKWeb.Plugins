using System;
using System.Collections.Concurrent;
using System.FastReflection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.TypeTraits {
	/// <summary>
	/// 类型是否可回收的特征类
	/// </summary>
	public class RecyclableTrait {
		/// <summary>
		/// 成员名称
		/// </summary>
		public string PropertyName { get; set; }
		/// <summary>
		/// 是否可回收
		/// </summary>
		public bool IsRecyclable { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public RecyclableTrait() {
			PropertyName = "Deleted";
			IsRecyclable = false;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="type">类型</param>
		public RecyclableTrait(Type type) : this() {
			IsRecyclable = type.FastGetProperty(PropertyName) != null;
		}

		/// <summary>
		/// 默认特征类的缓存
		/// </summary>
		private static ConcurrentDictionary<Type, RecyclableTrait> DefaultTraits =
			new ConcurrentDictionary<Type, RecyclableTrait>();

		/// <summary>
		/// 返回指定类型是否可回收的特征
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public static RecyclableTrait For(Type type) {
			var trait = Application.Ioc.Resolve<RecyclableTrait>(
				serviceKey: type, ifUnresolved: IfUnresolved.ReturnDefault);
			if (trait == null) {
				trait = DefaultTraits.GetOrAdd(type, t => new RecyclableTrait(t));
			}
			return trait;
		}

		/// <summary>
		/// 返回指定类型是否可回收的特征
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		public static RecyclableTrait For<T>() {
			return For(typeof(T));
		}
	}
}
