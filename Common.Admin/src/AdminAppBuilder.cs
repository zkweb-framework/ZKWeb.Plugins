using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Admin.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src {
	/// <summary>
	/// 后台应用构建器
	/// 支持自动生成列表和增删查改页面（Scaffold，半自动）
	/// 例子
	///		...
	/// </summary>
	/// <typeparam name="T">管理的数据类型</typeparam>
	[ExportMany]
	public abstract class AdminAppBuilder<T> : AdminApp, IAdminAppBuilder {
		/// <summary>
		/// 搜索请求使用的Url
		/// </summary>
		public const string SearchUrl = "/search";
		/// <summary>
		/// 添加数据使用的Url
		/// </summary>
		public const string AddUrl = "/add";
		/// <summary>
		/// 编辑使用的Url
		/// </summary>
		public const string EditUrl = "/edit";
		/// <summary>
		/// 批量操作使用的Url
		/// </summary>
		public const string BatchUrl = "/batch";

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult ListAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 搜索请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult SearchAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 添加页和添加请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 编辑页和编辑请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 批量操作请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult BatchAction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, ListAction);
			controllerManager.RegisterAction(Url + SearchUrl, HttpMethods.POST, SearchAction);
			controllerManager.RegisterAction(Url + AddUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(Url + AddUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(Url + EditUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(Url + EditUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(Url + BatchUrl, HttpMethods.POST, BatchAction);
		}
	}
}
