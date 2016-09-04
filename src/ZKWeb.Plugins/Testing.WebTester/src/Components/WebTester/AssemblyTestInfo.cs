using System;
using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.Plugins.Testing.WebTester.src.Components.WebTester.Enums;
using ZKWebStandard.Extensions;
using ZKWebStandard.Testing;

namespace ZKWeb.Plugins.Testing.WebTester.src.Components.WebTester {
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
		/// 测试结果的计数器
		/// </summary>
		public TestResultCounter Counter { get; set; }
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
		public string UpdateTime { get; set; }

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
			Counter = new TestResultCounter();
			Updated();
		}

		/// <summary>
		/// 设置更新时间
		/// </summary>
		public void Updated() {
			UpdateTime = DateTime.UtcNow.Ticks.ToString();
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
			Counter = new TestResultCounter();
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
