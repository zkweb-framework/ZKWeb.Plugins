#if !NETCORE
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Pingpp.Exception;
using Pingpp.Models;
using Pingpp.Utils;
using ZKWebStandard.Utils;

namespace Pingpp.Net {
	internal class Requestor : Pingpp {
		internal static HttpWebRequest GetRequest(string path, string method, string sign) {
			var request = (HttpWebRequest)WebRequest.Create(ApiBase + path);
			request.Headers.Add("Authorization", string.Format("Bearer {0}", ApiKey));
			request.Headers.Add("Pingplusplus-Version", ApiVersion);
			request.Headers.Add("Accept-Language", AcceptLanguage);
			if (!string.IsNullOrEmpty(sign)) {
				request.Headers.Add("Pingplusplus-Signature", sign);
			}
			request.UserAgent = "Pingpp C# SDK version" + Version;
			request.ContentType = "application/json;charset=utf-8";
			request.Timeout = DefaultTimeout;
			request.ReadWriteTimeout = DefaultReadAndWriteTimeout;
			request.Method = method;

			return request;
		}

		internal static string DoRequest(string path, string method, Dictionary<string, object> param = null) {
			if (string.IsNullOrEmpty(ApiKey)) {
				throw new PingppException("No API key provided.  (HINT: set your API key using " +
				"\"Pingpp::setApiKey(<API-KEY>)\".  You can generate API keys from " +
				"the Pingpp web interface.  See https://pingxx.com/document/api for " +
				"details.");
			}
			try {
				HttpWebRequest req;
				HttpWebResponse res;
				method = method.ToUpper();
				switch (method) {
					case "GET":
					case "DELETE":
						req = GetRequest(path, method, "");
						using (res = req.GetResponse() as HttpWebResponse) {
							return res == null ? null : ReadStream(res.GetResponseStream());
						}
					case "POST":
					case "PUT":
						if (param == null) {
							throw new PingppException("Request params is empty");
						}
						var body = JsonConvert.SerializeObject(param, Formatting.Indented);
						string sign;
						try {
							sign = RsaUtils.RsaSign(body, PrivateKey);
						} catch (System.Exception e) {
							throw new PingppException("Sign request error." + e.Message);
						}
						req = GetRequest(path, method, sign);
						using (var streamWriter = new StreamWriter(req.GetRequestStream())) {
							streamWriter.Write(body);
							streamWriter.Flush();
							streamWriter.Close();
						}
						using (res = req.GetResponse() as HttpWebResponse) {
							return res == null ? null : ReadStream(res.GetResponseStream());
						}
					default:
						return null;
				}
			} catch (WebException e) {
				if (e.Response == null) throw new WebException(e.Message);
				var statusCode = ((HttpWebResponse)e.Response).StatusCode;
				var errors = Mapper<Error>.MapFromJson(ReadStream(e.Response.GetResponseStream()), "error");

				throw new PingppException(errors, errors.ErrorType, errors.Message);
			}
		}

		private static string ReadStream(Stream stream) {
			using (var reader = new StreamReader(stream, Encoding.UTF8)) {
				return reader.ReadToEnd();
			}
		}

		internal static Dictionary<string, string> FormatParams(Dictionary<string, object> param) {
			if (param == null) {
				return new Dictionary<string, string>();
			}
			var formattedParam = new Dictionary<string, string>();
			foreach (var dic in param) {
				var dicts = dic.Value as Dictionary<string, string>;
				if (dicts != null) {
					var formatNestedDic = new Dictionary<string, object>();
					foreach (var nestedDict in dicts) {
						formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, nestedDict.Key), nestedDict.Value);
					}

					foreach (var nestedDict in FormatParams(formatNestedDic)) {
						formattedParam.Add(nestedDict.Key, nestedDict.Value);
					}
				} else if (dic.Value is Dictionary<string, object>) {
					var formatNestedDic = new Dictionary<string, object>();

					foreach (var nestedDict in (Dictionary<string, object>)dic.Value) {
						formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, nestedDict.Key), nestedDict.Value.ToString());
					}

					foreach (var nestedDict in FormatParams(formatNestedDic)) {
						formattedParam.Add(nestedDict.Key, nestedDict.Value);
					}
				} else if (dic.Value is IList) {
					var li = (List<object>)dic.Value;
					var formatNestedDic = new Dictionary<string, object>();
					var size = li.Count();
					for (var i = 0; i < size; i++) {
						formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, i), li[i]);
					}
					foreach (var nestedDict in FormatParams(formatNestedDic)) {
						formattedParam.Add(nestedDict.Key, nestedDict.Value);
					}
				} else if ("".Equals(dic.Value)) {
					throw new PingppException(string.Format("You cannot set '{0}' to an empty string. " +
						"We interpret empty strings as null in requests. " +
						"You may set '{0}' to null to delete the property.", dic.Key));
				} else if (dic.Value == null) {
					formattedParam.Add(dic.Key, "");
				} else {
					formattedParam.Add(dic.Key, dic.Value.ToString());
				}

			}
			return formattedParam;
		}

		internal static string CreateQuery(Dictionary<string, object> param) {
			var flatParams = FormatParams(param);
			var queryStringBuffer = new StringBuilder();
			foreach (var entry in flatParams) {
				if (queryStringBuffer.Length > 0) {
					queryStringBuffer.Append("&");
				}

				queryStringBuffer.Append(UrlEncodePair(entry.Key, entry.Value));
			}
			return queryStringBuffer.ToString();
		}

		internal static string UrlEncodePair(string k, string v) {
			return string.Format("{0}={1}", UrlEncode(k), UrlEncode(v));
		}

		private static string UrlEncode(string str) {
			return string.IsNullOrEmpty(str) ? null : HttpUtils.UrlEncode(str);
		}

		internal static string FormatUrl(string url, string query) {
			return string.IsNullOrEmpty(query) ? url : string.Format("{0}{1}{2}", url, url.Contains("?") ? "&" : "?", query);
		}
	}
}
#endif
