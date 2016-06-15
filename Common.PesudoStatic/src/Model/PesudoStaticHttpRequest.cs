using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.PesudoStatic.src.Model {
	/// <summary>
	/// 伪静态使用的http请求
	/// 用于重载路径相关的成员
	/// </summary>
	internal class PesudoStaticHttpRequest : IHttpRequest {
		/// <summary>
		/// 原始的http请求
		/// </summary>
		public IHttpRequest OriginalRequest { get; protected set; }
		/// <summary>
		/// 重载的Url
		/// </summary>
		public Uri OverrideUrl { get; protected set; }
		/// <summary>
		/// 重载的Url参数
		/// </summary>
		public NameValueCollection OverrideQueryString { get; protected set; }
		/// <summary>
		/// 重载的所有参数
		/// </summary>
		public NameValueCollection OverrideParams { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="originalRequest">原始的http请求</param>
		/// <param name="overrideUrl">重载的Url</param>
		public PesudoStaticHttpRequest(IHttpRequest originalRequest, Uri overrideUrl) {
			OriginalRequest = originalRequest;
			OverrideUrl = overrideUrl;
			OverrideQueryString = HttpUtility.ParseQueryString(overrideUrl.Query);
			OverrideParams = null;
		}

		public override string this[string key] {
			get {
				string value = null;
				if ((value = QueryString[key]) != null) {
					return value;
				} else if ((value = Form[key]) != null) {
					return value;
				} else if (Cookies[key] != null) {
					return value;
				} else if ((value = ServerVariables[key]) != null) {
					return value;
				}
				return value;
			}
		}

		public override string[] AcceptTypes {
			get { return OriginalRequest.AcceptTypes; }
		}
		public override string AnonymousID {
			get { return OriginalRequest.AnonymousID; }
		}
		public override string ApplicationPath {
			get { return OriginalRequest.ApplicationPath; }
		}
		public override string AppRelativeCurrentExecutionFilePath {
			get { return OriginalRequest.AppRelativeCurrentExecutionFilePath; }
		}
		public override HttpBrowserCapabilitiesBase Browser {
			get { return OriginalRequest.Browser; }
		}
		public override HttpClientCertificate ClientCertificate {
			get { return OriginalRequest.ClientCertificate; }
		}
		public override Encoding ContentEncoding {
			get { return OriginalRequest.ContentEncoding; }
			set { OriginalRequest.ContentEncoding = value; }
		}
		public override int ContentLength {
			get { return OriginalRequest.ContentLength; }
		}
		public override string ContentType {
			get { return OriginalRequest.ContentType; }
			set { OriginalRequest.ContentType = value; }
		}
		public override HttpCookieCollection Cookies {
			get { return OriginalRequest.Cookies; }
		}
		public override string CurrentExecutionFilePath {
			get { return OriginalRequest.CurrentExecutionFilePath; }
		}
		public override string CurrentExecutionFilePathExtension {
			get { return OriginalRequest.CurrentExecutionFilePathExtension; }
		}
		public override string FilePath {
			get { return OriginalRequest.FilePath; }
		}
		public override HttpFileCollectionBase Files {
			get { return OriginalRequest.Files; }
		}
		public override Stream Filter {
			get { return OriginalRequest.Filter; }
			set { OriginalRequest.Filter = value; }
		}
		public override NameValueCollection Form {
			get { return OriginalRequest.Form; }
		}
		public override NameValueCollection Headers {
			get { return OriginalRequest.Headers; }
		}
		public override ChannelBinding HttpChannelBinding {
			get { return OriginalRequest.HttpChannelBinding; }
		}
		public override string HttpMethod {
			get { return OriginalRequest.HttpMethod; }
		}
		public override Stream InputStream {
			get { return OriginalRequest.InputStream; }
		}
		public override bool IsAuthenticated {
			get { return OriginalRequest.IsAuthenticated; }
		}
		public override bool IsLocal {
			get { return OriginalRequest.IsLocal; }
		}
		public override bool IsSecureConnection {
			get { return OriginalRequest.IsSecureConnection; }
		}
		public override WindowsIdentity LogonUserIdentity {
			get { return OriginalRequest.LogonUserIdentity; }
		}
		public override NameValueCollection Params {
			get {
				if (OverrideParams == null) {
					var overrideParams = new NameValueCollection();
					overrideParams.Add(QueryString);
					overrideParams.Add(Form);
					Cookies.OfType<HttpCookie>().ForEach(c => overrideParams.Add(c.Name, c.Value));
					overrideParams.Add(ServerVariables);
					OverrideParams = overrideParams;
				}
				return OverrideParams;
			}
		}
		public override string Path {
			get { return OverrideUrl.AbsolutePath; }
		}
		public override string PathInfo {
			get { return OriginalRequest.PathInfo; }
		}
		public override string PhysicalApplicationPath {
			get { return OriginalRequest.PhysicalApplicationPath; }
		}
		public override string PhysicalPath {
			get { return OriginalRequest.PhysicalPath; }
		}
		public override NameValueCollection QueryString {
			get { return OverrideQueryString; }
		}
		public override string RawUrl {
			get { return OverrideUrl.AbsolutePath; }
		}
		public override ReadEntityBodyMode ReadEntityBodyMode {
			get { return OriginalRequest.ReadEntityBodyMode; }
		}
		public override RequestContext RequestContext {
			get { return OriginalRequest.RequestContext; }
			set { OriginalRequest.RequestContext = value; }
		}
		public override string RequestType {
			get { return OriginalRequest.RequestType; }
			set { OriginalRequest.RequestType = value; }
		}
		public override NameValueCollection ServerVariables {
			get { return OriginalRequest.ServerVariables; }
		}
		public override CancellationToken TimedOutToken {
			get { return OriginalRequest.TimedOutToken; }
		}
		public override int TotalBytes {
			get { return OriginalRequest.TotalBytes; }
		}
		public override UnvalidatedRequestValuesBase Unvalidated {
			get { return OriginalRequest.Unvalidated; }
		}
		public override Uri Url {
			get { return OverrideUrl; }
		}
		public override Uri UrlReferrer {
			get { return OriginalRequest.UrlReferrer; }
		}
		public override string UserAgent {
			get { return OriginalRequest.UserAgent; }
		}
		public override string UserHostAddress {
			get { return OriginalRequest.UserHostAddress; }
		}
		public override string UserHostName {
			get { return OriginalRequest.UserHostName; }
		}
		public override string[] UserLanguages {
			get { return OriginalRequest.UserLanguages; }
		}

		public override void Abort() {
			OriginalRequest.Abort();
		}
		public override byte[] BinaryRead(int count) {
			return OriginalRequest.BinaryRead(count);
		}
		public override Stream GetBufferedInputStream() {
			return OriginalRequest.GetBufferedInputStream();
		}
		public override Stream GetBufferlessInputStream() {
			return OriginalRequest.GetBufferedInputStream();
		}
		public override Stream GetBufferlessInputStream(bool disableMaxRequestLength) {
			return OriginalRequest.GetBufferlessInputStream(disableMaxRequestLength);
		}
		public override void InsertEntityBody() {
			OriginalRequest.InsertEntityBody();
		}
		public override void InsertEntityBody(byte[] buffer, int offset, int count) {
			OriginalRequest.InsertEntityBody(buffer, offset, count);
		}
		public override int[] MapImageCoordinates(string imageFieldName) {
			return OriginalRequest.MapImageCoordinates(imageFieldName);
		}
		public override string MapPath(string virtualPath) {
			return OriginalRequest.MapPath(virtualPath);
		}
		public override string MapPath(
			string virtualPath, string baseVirtualDir, bool allowCrossAppMapping) {
			return OriginalRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping);
		}
		public override double[] MapRawImageCoordinates(string imageFieldName) {
			return OriginalRequest.MapRawImageCoordinates(imageFieldName);
		}
		public override void SaveAs(string filename, bool includeHeaders) {
			OriginalRequest.SaveAs(filename, includeHeaders);
		}
		public override void ValidateInput() {
			OriginalRequest.ValidateInput();
		}
	}
}
