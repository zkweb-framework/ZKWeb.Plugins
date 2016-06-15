using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Model {
	/// <summary>
	/// 程序集的测试状态
	/// </summary>
	public enum AssemblyTestState {
		/// <summary>
		/// 未运行
		/// </summary>
		NotRunning = 0,
		/// <summary>
		/// 等待运行
		/// </summary>
		WaitingToRun = 1,
		/// <summary>
		/// 运行中
		/// </summary>
		Running = 2,
		/// <summary>
		/// 运行完毕
		/// </summary>
		FinishedRunning = 3
	}
}
