using NSubstitute;
using System;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.PesudoStatic.src.Config;
using ZKWeb.Plugins.Common.PesudoStatic.src.Model;
using ZKWeb.Plugins.Common.PesudoStatic.src.UrlFilters;
using ZKWebStandard.Testing;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Tests.UrlFilters {
	[Tests]
	class PesudoStaticUrlFilterTest {
		public void Filter() {
			var settings = new PesudoStaticSettings();
			settings.IncludeUrlPaths.Add("/include/me");
			settings.ExcludeUrlPaths.Add("/exclude/me");
			using (Application.OverrideIoc()) {
				var filter = new PesudoStaticUrlFilter();
				var configManagerMock = Substitute.For<GenericConfigManager>();
				configManagerMock.GetData<PesudoStaticSettings>().Returns(settings);
				Application.Ioc.Unregister<GenericConfigManager>();
				Application.Ioc.RegisterInstance(configManagerMock);
				var testUrl = new Func<string, string>(url => { filter.Filter(ref url); return url; });

				Assert.Equals(testUrl(""), "");
				Assert.Equals(testUrl("/"), "/");
				Assert.Equals(testUrl("#abc"), "#abc");
				Assert.Equals(testUrl("http://localhost/"), "http://localhost/");
				Assert.Equals(testUrl("ftp://localhost/"), "ftp://localhost/");

				Assert.Equals(testUrl("/test/abc.jpg"), "/test/abc.jpg");
				Assert.Equals(testUrl("/test/abc/"), "/test/abc/");
				Assert.Equals(testUrl("/test/abc/a-b?id=1"), "/test/abc/a-b?id=1");
				Assert.Equals(testUrl("/test/a-bc/ab?id=1"), "/test/a-bc/ab-1.html");

				Assert.Equals(testUrl("/exclude/me"), "/exclude/me");
				settings.PesudoStaticPolicy = PesudoStaticPolicies.WhiteListPolicy;
				Assert.Equals(testUrl("/include/me"), "/include/me.html");
				Assert.Equals(testUrl("/otherwise"), "/otherwise");
				settings.PesudoStaticPolicy = PesudoStaticPolicies.BlackListPolicy;

				settings.EnablePesudoStatic = false;
				Assert.Equals(testUrl("/no_enabled"), "/no_enabled");
				settings.EnablePesudoStatic = true;
				Assert.Equals(testUrl("/enabled"), "/enabled.html");

				Assert.Equals(testUrl("login"), "login.html");
				Assert.Equals(testUrl("/example/view?id=1"), "/example/view-1.html");
				Assert.Equals(testUrl("/example/view?Id=1"), "/example/view-Id-1.html");

				var result = testUrl("/example/view?tag=123&key=asd");
				Assert.IsTrueWith(
					(result == "/example/view-tag-123-key-asd.html" ||
					result == "/example/view-key-asd-tag-123.html"), result);
				Assert.Equals(testUrl("view?name=john&key=1-1"), "view-name-john.html?key=1-1");
				Assert.Equals(testUrl("view?name=john&key=%26"), "view-name-john.html?key=%26");
			}
		}
	}
}
