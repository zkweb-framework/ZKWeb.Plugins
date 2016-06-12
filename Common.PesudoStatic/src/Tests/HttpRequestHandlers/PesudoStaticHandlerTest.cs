using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.PesudoStatic.src.Config;
using ZKWeb.Plugins.Common.PesudoStatic.src.HttpRequestHandlers;
using ZKWeb.Utils.Functions;
using ZKWeb.Utils.IocContainer;
using ZKWeb.Utils.UnitTest;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Tests.HttpRequestHandlers {
	[UnitTest]
	class PesudoStaticHandlerTest {
		public void OnRequest() {
			var settings = new PesudoStaticSettings();
			using (Application.OverrideIoc()) {
				var configManagerMock = Substitute.For<GenericConfigManager>();
				configManagerMock.GetData<PesudoStaticSettings>().Returns(settings);
				Application.Ioc.Unregister<GenericConfigManager>();
				Application.Ioc.RegisterInstance(configManagerMock);
				var testUrl = new Func<string, string>(url => {
					string parsedUrl = null;
					var handlerMock = Substitute.For<IHttpRequestHandler>();
					handlerMock.When(h => h.OnRequest()).Do(callInfo => {
						if (parsedUrl == null) {
							parsedUrl = HttpContextUtils.CurrentContext.Request.Url.PathAndQuery;
						}
					});
					Application.Ioc.Unregister<IHttpRequestHandler>();
					Application.Ioc.RegisterInstance(handlerMock);
					Application.Ioc.RegisterMany<PesudoStaticHandler>(ReuseType.Singleton);
					using (var context = HttpContextUtils.OverrideContext(url, "GET")) {
						var handlers = Application.Ioc.ResolveMany<IHttpRequestHandler>().Reverse();
						foreach (var handler in handlers) {
							handler.OnRequest();
						}
					}
					return parsedUrl;
				});

				Assert.Equals(testUrl("/"), "/");
				Assert.Equals(testUrl("/login.html"), "/login");
				Assert.Equals(testUrl("/user/login.html"), "/user/login");
				Assert.Equals(testUrl("/example/view-1.html"), "/example/view?id=1");
				Assert.Equals(testUrl("/example/view-abc.html"), "/example/view?id=abc");
				Assert.Equals(testUrl("/example/view-key-asd.html"), "/example/view?key=asd");

				var result = testUrl("/example/view-name-john.html?key=%26");
				Assert.IsTrueWith(
					(result == "/example/view?name=john&key=%26" ||
					result == "/example/view?key=%26&name=john"), result);
				Assert.Equals(testUrl("/test-param-nums-error.html"), "/test-param-nums-error.html");
			}
		}
	}
}
