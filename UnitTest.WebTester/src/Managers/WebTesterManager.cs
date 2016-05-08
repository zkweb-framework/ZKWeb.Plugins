using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}
}
