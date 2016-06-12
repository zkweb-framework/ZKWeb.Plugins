using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Instrumentation;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.WebSockets;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Model {
	/// <summary>
	/// 伪静态使用的http上下文
	/// 用于重载请求
	/// </summary>
	internal class PesudoStaticHttpContext : HttpContextBase {
		/// <summary>
		/// 原始的http上下文
		/// </summary>
		public HttpContextBase OriginalContext { get; protected set; }
		/// <summary>
		/// 重载的http请求
		/// </summary>
		public HttpRequestBase OverrideRequest { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalContext">原始的http上下文</param>
		/// <param name="overrideRequest">重载的http请求</param>
		public PesudoStaticHttpContext(
			HttpContextBase originalContext, HttpRequestBase overrideRequest) {
			OriginalContext = originalContext;
			OverrideRequest = overrideRequest;
		}

		public override Exception[] AllErrors {
			get { return OriginalContext.AllErrors; }
		}
		public override bool AllowAsyncDuringSyncStages {
			get { return OriginalContext.AllowAsyncDuringSyncStages; }
			set { OriginalContext.AllowAsyncDuringSyncStages = value; }
		}
		public override HttpApplicationStateBase Application {
			get { return OriginalContext.Application; }
		}
		public override HttpApplication ApplicationInstance {
			get { return OriginalContext.ApplicationInstance; }
			set { OriginalContext.ApplicationInstance = value; }
		}
		public override AsyncPreloadModeFlags AsyncPreloadMode {
			get { return OriginalContext.AsyncPreloadMode; }
			set { OriginalContext.AsyncPreloadMode = value; }
		}
		public override System.Web.Caching.Cache Cache {
			get { return OriginalContext.Cache; }
		}
		public override IHttpHandler CurrentHandler {
			get { return OriginalContext.CurrentHandler; }
		}
		public override RequestNotification CurrentNotification {
			get { return OriginalContext.CurrentNotification; }
		}
		public override Exception Error {
			get { return OriginalContext.Error; }
		}
		public override IHttpHandler Handler {
			get { return OriginalContext.Handler; }
			set { OriginalContext.Handler = value; }
		}
		public override bool IsCustomErrorEnabled {
			get { return OriginalContext.IsCustomErrorEnabled; }
		}
		public override bool IsDebuggingEnabled {
			get { return OriginalContext.IsDebuggingEnabled; }
		}
		public override bool IsPostNotification {
			get { return OriginalContext.IsPostNotification; }
		}
		public override bool IsWebSocketRequest {
			get { return OriginalContext.IsWebSocketRequest; }
		}
		public override bool IsWebSocketRequestUpgrading {
			get { return OriginalContext.IsWebSocketRequestUpgrading; }
		}
		public override IDictionary Items {
			get { return OriginalContext.Items; }
		}
		public override PageInstrumentationService PageInstrumentation {
			get { return OriginalContext.PageInstrumentation; }
		}
		public override IHttpHandler PreviousHandler {
			get { return OriginalContext.PreviousHandler; }
		}
		public override ProfileBase Profile {
			get { return OriginalContext.Profile; }
		}
		public override HttpRequestBase Request {
			get { return OverrideRequest; }
		}
		public override HttpResponseBase Response {
			get { return OriginalContext.Response; }
		}
		public override HttpServerUtilityBase Server {
			get { return OriginalContext.Server; }
		}
		public override HttpSessionStateBase Session {
			get { return OriginalContext.Session; }
		}
		public override bool SkipAuthorization {
			get { return OriginalContext.SkipAuthorization; }
			set { OriginalContext.SkipAuthorization = value; }
		}
		public override bool ThreadAbortOnTimeout {
			get { return OriginalContext.ThreadAbortOnTimeout; }
			set { OriginalContext.ThreadAbortOnTimeout = value; }
		}
		public override DateTime Timestamp {
			get { return OriginalContext.Timestamp; }
		}
		public override TraceContext Trace {
			get { return OriginalContext.Trace; }
		}
		public override IPrincipal User {
			get { return OriginalContext.User; }
			set { OriginalContext.User = value; }
		}
		public override string WebSocketNegotiatedProtocol {
			get { return OriginalContext.WebSocketNegotiatedProtocol; }
		}
		public override IList<string> WebSocketRequestedProtocols {
			get { return OriginalContext.WebSocketRequestedProtocols; }
		}

		public override void AcceptWebSocketRequest(Func<AspNetWebSocketContext, Task> userFunc) {
			OriginalContext.AcceptWebSocketRequest(userFunc);
		}
		public override void AcceptWebSocketRequest(
			Func<AspNetWebSocketContext, Task> userFunc, AspNetWebSocketOptions options) {
			OriginalContext.AcceptWebSocketRequest(userFunc, options);
		}
		public override void AddError(Exception errorInfo) {
			OriginalContext.AddError(errorInfo);
		}
		public override ISubscriptionToken AddOnRequestCompleted(Action<HttpContextBase> callback) {
			return OriginalContext.AddOnRequestCompleted(callback);
		}
		public override void ClearError() {
			OriginalContext.ClearError();
		}
		public override ISubscriptionToken DisposeOnPipelineCompleted(IDisposable target) {
			return OriginalContext.DisposeOnPipelineCompleted(target);
		}
		public override object GetGlobalResourceObject(string classKey, string resourceKey) {
			return OriginalContext.GetGlobalResourceObject(classKey, resourceKey);
		}
		public override object GetGlobalResourceObject(
			string classKey, string resourceKey, CultureInfo culture) {
			return OriginalContext.GetGlobalResourceObject(classKey, resourceKey, culture);
		}
		public override object GetLocalResourceObject(string virtualPath, string resourceKey) {
			return OriginalContext.GetLocalResourceObject(virtualPath, resourceKey);
		}
		public override object GetLocalResourceObject(
			string virtualPath, string resourceKey, CultureInfo culture) {
			return OriginalContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
		}
		public override object GetSection(string sectionName) {
			return OriginalContext.GetSection(sectionName);
		}
		public override object GetService(Type serviceType) {
			return OriginalContext.GetService(serviceType);
		}
		public override void RemapHandler(IHttpHandler handler) {
			OriginalContext.RemapHandler(handler);
		}
		public override void RewritePath(string path) {
			OriginalContext.RewritePath(path);
		}
		public override void RewritePath(string path, bool rebaseClientPath) {
			OriginalContext.RewritePath(path, rebaseClientPath);
		}
		public override void RewritePath(string filePath, string pathInfo, string queryString) {
			OriginalContext.RewritePath(filePath, pathInfo, queryString);
		}
		public override void RewritePath(
			string filePath, string pathInfo, string queryString, bool setClientFilePath) {
			OriginalContext.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
		}
		public override void SetSessionStateBehavior(SessionStateBehavior sessionStateBehavior) {
			OriginalContext.SetSessionStateBehavior(sessionStateBehavior);
		}
	}
}
