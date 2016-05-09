using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.UnitTest;

namespace ZKWeb.Plugins.UnitTest.WebTester.src.Tests {
	[UnitTest]
	class CustomTest {
		public void TestFailed() {
			Assert.IsTrueWith(false, "just test it could failed");
		}

		public void TestMoreFailed() {
			Assert.IsTrueWith(false, "just test it could failed");
		}

		public void TestSkipped() {
			Assert.Skiped("just test it could skipped");
		}

		public void TestMoreSkipped() {
			Assert.Skiped("just test it could skipped");
		}

		public void TestPassed() {
			UnitTestRunner.CurrentRunner.WriteDebugMessage("test debug message a");
			UnitTestRunner.CurrentRunner.WriteDebugMessage("test debug message b");
			UnitTestRunner.CurrentRunner.WriteErrorMessage("test error message a");
			UnitTestRunner.CurrentRunner.WriteErrorMessage("test error message b");
			Assert.Passed();
		}
	}
}
