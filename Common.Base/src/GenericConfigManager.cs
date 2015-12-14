using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 通用配置管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericConfigManager {
		/// <summary>
		/// 获取指定类型的所属插件 + 配置键名
		/// 用于从数据库中获取对应的配置
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <returns></returns>
		private static string GetKey<T>() {
			var attribute = typeof(T).GetAttribute<GenericConfigAttribute>();
			if (attribute == null) {
				throw new ArgumentException(string.Format(
					"type {0} not contains GenericConfigAttribute", typeof(T).Name));
			}
			return attribute.Plugin + "." + attribute.Key;
		}

		/// <summary>
		/// 储存配置值
		/// </summary>
		public void PutData<T>(T value) where T : class, new() {
			var key = GetKey<T>();
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				var config = context.Get<GenericConfig>(c => c.Key == key);
				config = config ?? new GenericConfig() { Key = key };
				context.Save(ref config, c => {
					c.Value = JsonConvert.SerializeObject(value);
					c.LastUpdated = DateTime.UtcNow;
				});
				context.SaveChanges();
			}
		}

		/// <summary>
		/// 获取配置值，没有找到时返回新的值
		/// </summary>
		/// <returns></returns>
		public T GetData<T>() where T : class, new() {
			var key = GetKey<T>();
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				var config = context.Get<GenericConfig>(c => c.Key == key);
				if (config == null) {
					return new T();
				} else {
					return JsonConvert.DeserializeObject<T>(config.Value) ?? new T();
				}
			}
		}

		/// <summary>
		/// 删除配置值
		/// </summary>
		public void RemoveData<T>() {
			var key = GetKey<T>();
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				context.DeleteWhere<GenericConfig>(c => c.Key == key);
				context.SaveChanges();
			}
		}
	}
}
