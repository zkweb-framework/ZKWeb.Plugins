using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;

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
		/// 状态名称
		/// </summary>
		public string StateName { get { return new T(State.GetDescription()); } }
		/// <summary>
		/// 通过数量
		/// </summary>
		public ulong Passed { get; set; }
		/// <summary>
		/// 跳过数量
		/// </summary>
		public ulong Skipped { get; set; }
		/// <summary>
		/// 失败数量
		/// </summary>
		public ulong Failed { get; set; }
		/// <summary>
		/// 跳过时输出的信息
		/// </summary>
		public string SkippedMessage { get; set; }
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
		/// 更新时间 (Ticks)
		/// </summary>
		public string LastUpdated { get; set; }

		/// <summary>
		/// 初始化，反序列化时使用
		/// </summary>
		public AssemblyTestInfo() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="assembly">程序集</param>
		public AssemblyTestInfo(Assembly assembly) {
			AssemblyName = assembly.GetName().Name;
			State = AssemblyTestState.NotRunning;
			Updated();
		}

		/// <summary>
		/// 设置更新时间
		/// </summary>
		public void Updated() {
			LastUpdated = DateTime.UtcNow.Ticks.ToString();
		}

		/// <summary>
		/// 判断信息是否可重置
		/// </summary>
		public bool Resetable() {
			return State == AssemblyTestState.FinishedRunning;
		}

		/// <summary>
		/// 重置信息，仅运行完毕后可以执行这个函数
		/// </summary>
		public void Reset() {
			if (!Resetable()) {
				throw new NotSupportedException("test information is not resetable now");
			}
			State = AssemblyTestState.NotRunning;
			Passed = 0;
			Skipped = 0;
			Failed = 0;
			SkippedMessage = null;
			FailedMessage = null;
			ErrorMessage = null;
			DebugMessage = null;
			Updated();
		}

		/// <summary>
		/// 尝试等待开始运行
		/// 返回是否成功设置了状态
		/// </summary>
		/// <returns></returns>
		public bool TryWaitingToRun() {
			if (Resetable()) {
				Reset();
			}
			if (State == AssemblyTestState.NotRunning) {
				State = AssemblyTestState.WaitingToRun;
				Updated();
				return true;
			}
			return false;
		}
	}
}
