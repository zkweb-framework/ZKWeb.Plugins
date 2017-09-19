using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.PesudoStatic.src.Components.HttpRequestHandlers;
using ZKWeb.Web;
using ZKWebStandard.Testing;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Tests.HttpRequestHandlers {
	[Tests]
	class PesudoStaticHandlerTest {
		public class GenericConfigManagerMock : GenericConfigManager {
			private PesudoStaticSettings _settings;

			public GenericConfigManagerMock(PesudoStaticSettings settings) {
				_settings = settings;
			}

			public override T GetData<T>() {
				if (typeof(T) == typeof(PesudoStaticSettings)) {
					return (T)(object)_settings;
				}
				return base.GetData<T>();
			}
		}

		public void OnRequest() {
			var settings = new PesudoStaticSettings();
			using (Application.OverrideIoc()) {
				Application.Ioc.Unregister<GenericConfigManager>();
				Application.Ioc.RegisterInstance<GenericConfigManager>(
					new GenericConfigManagerMock(new PesudoStaticSettings()));
				var testUrl = new Func<string, string>(url => {
					string parsedUrl = null;
					var wrapper = new PesudoStaticHandlerWrapper();
					using (HttpManager.OverrideContext(url, HttpMethods.GET)) {
						wrapper.WrapHandlerAction(() => {
							var request = HttpManager.CurrentContext.Request;
							parsedUrl = request.Path + request.QueryString;
						})();
					}
					return parsedUrl;
				});

				Assert.Equals(testUrl("/"), "/");
				Assert.Equals(testUrl("/login.html"), "/login");
				Assert.Equals(testUrl("/user/login.html"), "/user/login");
				Assert.Equals(testUrl("/example/view.1.html"), "/example/view?id=1");
				Assert.Equals(testUrl("/example/view.abc.html"), "/example/view?id=abc");
				Assert.Equals(testUrl("/example/view.key.asd.html"), "/example/view?key=asd");

				var result = testUrl("/example/view.name.john.html?key=%26");
				Assert.IsTrueWith(
					(result == "/example/view?name=john&key=%26" ||
					result == "/example/view?key=%26&name=john"), result);
				result = testUrl("/example/view.name.john.age.50.html");
				Assert.IsTrueWith(
					(result == "/example/view?name=john&age=50" ||
					result == "/example/view?age=50&name=john"), result);
				Assert.Equals(testUrl("/test.param.nums.error.html"), "/test.param.nums.error.html");
			}
		}
	}
}
