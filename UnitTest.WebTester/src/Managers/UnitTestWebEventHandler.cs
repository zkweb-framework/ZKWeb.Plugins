using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.UnitTest.Event;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Managers {
	/// <summary>
	/// 在网页中运行单元测试使用的事件处理器
	/// </summary>
	public class UnitTestWebEventHandler : IUnitTestEventHandler {
		public void OnAllTestStarting(AllTestStartingInfo info) {
			throw new NotImplementedException();
		}

		public void OnAllTestCompleted(AllTestCompletedInfo info) {
			throw new NotImplementedException();
		}

		public void OnDebugMessage(DebugMessageInfo info) {
			throw new NotImplementedException();
		}

		public void OnErrorMessage(ErrorMessageInfo info) {
			throw new NotImplementedException();
		}

		public void OnTestFailed(TestFailedInfo info) {
			throw new NotImplementedException();
		}

		public void OnTestPassed(TestPassedInfo info) {
			throw new NotImplementedException();
		}

		public void OnTestSkipped(TestSkippedInfo info) {
			throw new NotImplementedException();
		}

		public void OnTestStarting(TestStartingInfo info) {
			throw new NotImplementedException();
		}
	}
}
