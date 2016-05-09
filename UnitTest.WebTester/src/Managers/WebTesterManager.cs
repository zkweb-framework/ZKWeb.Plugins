using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Plugins.UnitTest.WebTester.src.Model;
using ZKWeb.UnitTest;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Managers {
	/// <summary>
	/// 网页上运行单元测试使用的管理器
	/// 
	/// 单元测试运行流程
	/// - 检查单元测试线程是否已启动，未启动时启动
	/// - 把需要测试的程序集加到队列中
	/// - 单元测试线程运行队列中的测试，并设置测试的状态
	///   测试的状态已程序集为单位
	/// 
	/// 单元测试的状态全局共享并可以随时获取
	/// 单元测试的状态可手动重置，仅限于状态是"未运行"
	/// </summary>
	[ExportMany, SingletonReuse]
	public class WebTesterManager {
		/// <summary>
		/// 测试信息列表
		/// 全局唯一，创建后不应该进行更改
		/// </summary>
		protected IEnumerable<AssemblyTestInfo> Informations { get; set; }
		/// <summary>
		/// 测试信息列表使用的线程锁
		/// </summary>
		protected object InformationsLock { get; set; }
		/// <summary>
		/// 测试线程
		/// </summary>
		protected Thread RunningThread { get; set; }
		/// <summary>
		/// 测试线程使用的锁
		/// </summary>
		protected object RunningThreadLock { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public WebTesterManager() {
			var unitTestManager = Application.Ioc.Resolve<UnitTestManager>();
			var assemblies = unitTestManager.GetAssembliesForTest();
			Informations = assemblies.Select(a => new AssemblyTestInfo(a)).ToList();
			InformationsLock = new object();
			RunningThread = null;
			RunningThreadLock = new object();
		}

		/// <summary>
		/// 检查测试线程是否已运行，未运行时运行
		/// </summary>
		protected virtual void EnsureRunningThread() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 测试线程的运行函数
		/// </summary>
		protected virtual void RunningThreadBody() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取当前的测试信息
		/// 返回的列表是克隆后的内容
		/// </summary>
		/// <returns></returns>
		public virtual IList<AssemblyTestInfo> GetInformations() {
			return GetInformations(new Dictionary<string, DateTime>());
		}

		/// <summary>
		/// 获取当前的测试信息，只获取差异部分
		/// 返回的列表是克隆后的内容
		/// </summary>
		/// <param name="lastUpdateds">{ 程序集: 更新时间 }</param>
		/// <returns></returns>
		public virtual IList<AssemblyTestInfo> GetInformations(
			IDictionary<string, DateTime> lastUpdateds) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 重置所有测试信息
		/// </summary>
		public virtual void ResetInformations() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 更新指定的测试信息
		/// </summary>
		/// <param name="assemblyName">程序集名称</param>
		/// <param name="action">更新函数</param>
		public virtual void UpdateInformation(string assemblyName, Action<AssemblyTestInfo> action) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 后台运行指定的测试
		/// </summary>
		/// <param name="assemblyName">程序集名称</param>
		public virtual void RunAssemblyTestBackground(string assemblyName) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 后台运行所有测试
		/// </summary>
		public virtual void RunAllAssemblyTestBackground() {
			throw new NotImplementedException();
		}
	}
}
