using NSubstitute;
using System;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.HttpRequestHandlers;
using ZKWeb.Web;
using ZKWebStandard.Ioc;
using ZKWebStandard.Testing;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Tests.HttpRequestHandlers {
	[Tests]
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
							var request = HttpManager.CurrentContext.Request;
							parsedUrl = request.Path + request.QueryString;
						}
					});
					Application.Ioc.Unregister<IHttpRequestHandler>();
					Application.Ioc.RegisterInstance(handlerMock);
					Application.Ioc.RegisterMany<PesudoStaticHandler>(ReuseType.Singleton);
					using (var context = HttpManager.OverrideContext(url, "GET")) {
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
				result = testUrl("/example/view-name-john-age-50.html");
				Assert.IsTrueWith(
					(result == "/example/view?name=john&age=50" ||
					result == "/example/view?age=50&name=john"), result);
				Assert.Equals(testUrl("/test-param-nums-error.html"), "/test-param-nums-error.html");
			}
		}
	}
}
