using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
#if !NETCORE
using System.Threading;
#endif
using ZKWeb;
using ZKWeb.Logging;
using ZKWeb.Storage;

namespace WxPayAPI {
	/// <summary>
	/// http连接基础类，负责底层的http通信
	/// </summary>
	public class WxPayHttpService {
		/// <summary>
		/// 不检查https证书
		/// 微信原本的代码就要求这样，可能他们的证书有问题
		/// </summary>
		/// <returns></returns>
		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) {
			return true;
		}

		/// <summary>
		/// 提交xml到指定地址
		/// </summary>
		/// <returns></returns>
		public static string Post(
			WxPayConfig config, string xml, string url, bool isUseCert, int timeout) {
			string result = "";
			HttpWebRequest request = null;
			HttpWebResponse response = null;

			try {
				request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "text/xml";
				byte[] data = Encoding.UTF8.GetBytes(xml);
#if !NETCORE
				// 设置最大连接数
				ServicePointManager.DefaultConnectionLimit = 200;
				// 设置不验证服务端证书
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase)) {
					request.ServerCertificateValidationCallback += CheckValidationResult;
				}
				// 设置内容长度
				request.ContentLength = data.Length;
				// 设置超时时间
				if (timeout > 0) {
					request.Timeout = timeout * 1000;
				}
#endif
				// 是否使用证书
				if (isUseCert) {
					var localPathManager = Application.Ioc.Resolve<LocalPathManager>();
					var certPath = localPathManager.GetStorageFullPath(config.SSLCERT_PATH);
					var cert = new X509Certificate2(certPath, config.SSLCERT_PASSWORD);
#if !NETCORE
					request.ClientCertificates.Add(cert);
#else
					throw new WxPayException(".Net Core unsupport client certificates");
#endif
				}

				// 往服务器写入数据
				using (var reqStream = request.GetRequestStreamAsync().Result) {
					reqStream.Write(data, 0, data.Length);
				}

				// 获取服务端返回
				response = (HttpWebResponse)request.GetResponseAsync().Result;

				// 获取服务端返回数据
				using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
					result = sr.ReadToEnd().Trim();
				}
#if !NETCORE
			} catch (ThreadAbortException e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError("WxPay: Thread - caught ThreadAbortException - resetting.");
				logManager.LogError($"WxPay: Exception {e}");
				Thread.ResetAbort();
#endif
			} catch (WebException e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError($"WxPay: Exception {e}");
				throw new WxPayException(e.Message);
			} catch (Exception e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError($"WxPay: Exception {e}");
				throw new WxPayException(e.ToString());
			} finally {
				// 关闭连接和流
				if (response != null) {
					response.Dispose();
				}
				if (request != null) {
					request.Abort();
				}
			}
			return result;
		}

		/// <summary>
		/// 处理http GET请求，返回数据
		/// </summary>
		/// <param name="url">请求的url地址</param>
		/// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
		public static string Get(string url) {
			string result = "";
			HttpWebRequest request = null;
			HttpWebResponse response = null;

			// 请求url以获取数据
			try {
				request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
#if !NETCORE

				// 设置最大连接数
				ServicePointManager.DefaultConnectionLimit = 200;
				// 设置不验证服务端证书
				if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase)) {
					request.ServerCertificateValidationCallback += CheckValidationResult;
				}
#endif

				// 获取服务端返回
				response = (HttpWebResponse)request.GetResponseAsync().Result;

				// 获取服务端返回数据
				using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
					result = sr.ReadToEnd().Trim();
				}
#if !NETCORE
			} catch (ThreadAbortException e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError("WxPay: Thread - caught ThreadAbortException - resetting.");
				logManager.LogError($"WxPay: Exception {e}");
				Thread.ResetAbort();
#endif
			} catch (WebException e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError($"WxPay: Exception {e}");
				throw new WxPayException(e.Message);
			} catch (Exception e) {
				var logManager = Application.Ioc.Resolve<LogManager>();
				logManager.LogError($"WxPay: Exception {e}");
				throw new WxPayException(e.ToString());
			} finally {
				// 关闭连接和流
				if (response != null) {
					response.Dispose();
				}
				if (request != null) {
					request.Abort();
				}
			}
			return result;
		}
	}
}