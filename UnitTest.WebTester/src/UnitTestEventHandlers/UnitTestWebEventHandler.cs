﻿using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.UnitTest.WebTester.src.Managers;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.UnitTest.Event;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.UnitTestEventHandlers {
	/// <summary>
	/// 在网页中运行单元测试使用的事件处理器
	/// </summary>
	public class UnitTestWebEventHandler : IUnitTestEventHandler {
		/// <summary>
		/// 网页上运行单元测试使用的管理器
		/// </summary>
		protected WebTesterManager WebTesterManager { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public UnitTestWebEventHandler(WebTesterManager webTesterManager) {
			WebTesterManager = webTesterManager;
		}

		/// <summary>
		/// 程序集测试开始
		/// </summary>
		public void OnAllTestStarting(AllTestStartingInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.State = Model.AssemblyTestState.Running;
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 程序集测试完毕
		/// </summary>
		public void OnAllTestCompleted(AllTestCompletedInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.State = Model.AssemblyTestState.FinishedRunning;
				testInfo.Passed = info.Passed;
				testInfo.Skiped = info.Skiped;
				testInfo.Failed = info.Failed;
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 收到除错信息
		/// </summary>
		public void OnDebugMessage(DebugMessageInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.DebugMessage += info.Message;
				testInfo.DebugMessage += "\r\n";
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 收到错误信息
		/// </summary>
		public void OnErrorMessage(ErrorMessageInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.ErrorMessage += info.Message;
				testInfo.ErrorMessage += "\r\n";
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 单项测试失败时
		/// </summary>
		public void OnTestFailed(TestFailedInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.Failed += 1;
				testInfo.FailedMessage += string.Format(
					"Test {0} failed:\r\n{1}\r\n\r\n", info.Method.GetFullName(), info.Exception);
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 单项测试通过时
		/// </summary>
		public void OnTestPassed(TestPassedInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.Passed += 1;
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 单项测试跳过时
		/// </summary>
		public void OnTestSkipped(TestSkippedInfo info) {
			var assemblyName = info.Runner.Assembly.GetName().Name;
			WebTesterManager.UpdateInformation(assemblyName, testInfo => {
				testInfo.Skiped += 1;
				testInfo.SkipedMessage = string.Format(
					"Test {0} skiped: {1}\r\n\r\n", info.Method.GetFullName(), info.Exception.Message);
				testInfo.Updated();
			});
		}

		/// <summary>
		/// 单项测试开始时
		/// </summary>
		public void OnTestStarting(TestStartingInfo info) {

		}
	}
}