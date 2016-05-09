using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Model {
	/// <summary>
	/// 程序集的测试信息
	/// </summary>
	public class AssemblyTestInfo {
		/// <summary>
		/// 程序集名称
		/// </summary>
		public string AssemblyName { get; set; }
		/// <summary>
		/// 测试状态
		/// </summary>
		public AssemblyTestState State { get; set; }
		/// <summary>
		/// 通过数量
		/// </summary>
		public ulong Passed { get; set; }
		/// <summary>
		/// 跳过数量
		/// </summary>
		public ulong Skiped { get; set; }
		/// <summary>
		/// 失败数量
		/// </summary>
		public ulong Failed { get; set; }
		/// <summary>
		/// 跳过时输出的信息
		/// </summary>
		public string SkipedMessage { get; set; }
		/// <summary>
		/// 失败时输出的信息
		/// </summary>
		public string FailedMessage { get; set; }
		/// <summary>
		/// 错误信息
		/// </summary>
		public string ErrorMessage { get; set; }
		/// <summary>
		/// 除错信息
		/// </summary>
		public string DebugMessage { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime LastUpdated { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="assembly">程序集</param>
		public AssemblyTestInfo(Assembly assembly) {
			AssemblyName = assembly.GetName().Name;
			State = AssemblyTestState.NotRunning;
		}

		/// <summary>
		/// 设置更新时间
		/// </summary>
		public void Updated() {
			LastUpdated = DateTime.UtcNow.Truncate();
		}

		/// <summary>
		/// 重置信息，仅运行完毕后可以执行这个函数
		/// </summary>
		public void Reset() {
			if (State != AssemblyTestState.FinishedRunning) {
				throw new NotSupportedException("test information not resetable if state isn't finished running");
			}
			State = AssemblyTestState.NotRunning;
			Passed = 0;
			Skiped = 0;
			Failed = 0;
			SkipedMessage = null;
			FailedMessage = null;
			ErrorMessage = null;
			DebugMessage = null;
			Updated();
		}
	}
}
