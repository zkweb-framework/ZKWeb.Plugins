using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Managers {
	/// <summary>
	/// 通用配置管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigManager {
		/// <summary>
		/// 配置值的缓存
		/// </summary>
		protected MemoryCache<string, object> Cache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericConfigManager() {
			Cache = new MemoryCache<string, object>();
		}

		/// <summary>
		/// 获取类型标记的配置属性
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		private static GenericConfigAttribute GetConfigAttribute<T>() {
			var attribute = typeof(T).GetAttribute<GenericConfigAttribute>();
			if (attribute == null) {
				throw new ArgumentException(string.Format(
					"type {0} not contains GenericConfigAttribute", typeof(T).Name));
			}
			return attribute;
		}

		/// <summary>
		/// 储存配置值
		/// </summary>
		public virtual void PutData<T>(T value) where T : class, new() {
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			// 保存到数据库
			UnitOfWork.WriteData<GenericConfig>(r => {
				var config = r.Get(c => c.Key == key);
				config = config ?? new GenericConfig() { Key = key };
				r.Save(ref config, c => {
					c.Value = JsonConvert.SerializeObject(value);
					c.LastUpdated = DateTime.UtcNow;
				});
			});
			// 保存到缓存
			if (attribute.CacheTime > 0) {
				Cache.Put(key, value, TimeSpan.FromSeconds(attribute.CacheTime));
			}
		}

		/// <summary>
		/// 获取配置值，没有找到时返回新的值
		/// </summary>
		/// <returns></returns>
		public virtual T GetData<T>() where T : class, new() {
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			// 从缓存获取
			var value = Cache.GetOrDefault(key) as T;
			if (value != null) {
				return value;
			}
			// 从数据库获取，允许缓存时设置到缓存
			UnitOfWork.ReadData<GenericConfig>(r => {
				var config = r.Get(c => c.Key == key);
				if (config != null) {
					value = JsonConvert.DeserializeObject<T>(config.Value);
					if (attribute.CacheTime > 0) {
						Cache.Put(key, value, TimeSpan.FromSeconds(attribute.CacheTime));
					}
				}
			});
			return value ?? new T();
		}

		/// <summary>
		/// 删除配置值
		/// </summary>
		public virtual void RemoveData<T>() {
			var attribute = GetConfigAttribute<T>();
			var key = attribute.Key;
			// 从缓存删除
			Cache.Remove(key);
			// 从数据库删除
			UnitOfWork.WriteData<GenericConfig>(r => r.DeleteWhere(c => c.Key == key));
		}
	}
}
