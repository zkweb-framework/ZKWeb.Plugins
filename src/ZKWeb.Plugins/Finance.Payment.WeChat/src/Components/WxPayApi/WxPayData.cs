using System.Collections.Generic;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using ZKWeb;
using ZKWeb.Logging;

namespace WxPayAPI {
	/// <summary>
	/// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
	/// 在调用接口之前先填充各个字段的值，然后进行接口通信，
	/// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
	/// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
	/// </summary>
	public class WxPayData {
		/// <summary>
		/// 采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
		/// </summary>
		private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

		/// <summary>
		/// 设置某个字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void SetValue(string key, object value) {
			m_values[key] = value;
		}

		/// <summary>
		/// 根据字段名获取某个字段的值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetValue(string key) {
			object o = null;
			m_values.TryGetValue(key, out o);
			return o;
		}

		/// <summary>
		/// 判断某个字段是否已设置
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsSet(string key) {
			object o = null;
			m_values.TryGetValue(key, out o);
			if (null != o)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 将Dictionary转成xml
		/// </summary>
		/// <returns></returns>
		public string ToXml() {
			// 数据为空时不能转化为xml格式
			if (0 == m_values.Count) {
				throw new WxPayException("WxPayData数据为空!");
			}
			string xml = "<xml>";
			foreach (var pair in m_values) {
				// 字段值不能为null，会影响后续流程
				if (pair.Value == null) {
					throw new WxPayException("WxPayData内部含有值为null的字段!");
				}
				if (pair.Value.GetType() == typeof(int)) {
					xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
				} else if (pair.Value.GetType() == typeof(string)) {
					xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
				} else {
					// 除了string和int类型不能含有其他数据类型
					throw new WxPayException("WxPayData字段数据类型错误!");
				}
			}
			xml += "</xml>";
			return xml;
		}

		/// <summary>
		/// 将xml转为WxPayData对象并返回对象内部的数据
		/// </summary>
		/// <param name="config"></param>
		/// <param name="xml"></param>
		/// <returns></returns>
		public SortedDictionary<string, object> FromXml(WxPayConfig config, string xml) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			if (string.IsNullOrEmpty(xml)) {
				throw new WxPayException("将空的xml串转换为WxPayData不合法!");
			}
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			XmlNode xmlNode = xmlDoc.FirstChild; // 获取到根节点<xml>
			XmlNodeList nodes = xmlNode.ChildNodes;
			foreach (XmlNode xn in nodes) {
				XmlElement xe = (XmlElement)xn;
				m_values[xe.Name] = xe.InnerText; // 获取xml的键值对到WxPayData内部的数据中
			}
			if (!string.Equals(m_values["return_code"], "SUCCESS")) {
				return m_values; // 错误无签名
			}
			CheckSign(config); // 验证签名,不通过会抛异常
			return m_values;
		}

		/// <summary>
		/// Dictionary格式转化成url参数格式
		/// </summary>
		/// <returns></returns>
		public string ToUrl() {
			string buff = "";
			foreach (KeyValuePair<string, object> pair in m_values) {
				if (pair.Value == null) {
					throw new WxPayException("WxPayData内部含有值为null的字段!");
				}
				if (pair.Key != "sign" && pair.Value.ToString() != "") {
					buff += pair.Key + "=" + pair.Value + "&";
				}
			}
			buff = buff.Trim('&');
			return buff;
		}

		/// <summary>
		/// Dictionary格式化成Json
		/// </summary>
		/// <returns></returns>
		public string ToJson() {
			string jsonStr = JsonConvert.SerializeObject(m_values);
			return jsonStr;
		}

		/// <summary>
		/// values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
		/// </summary>
		/// <returns></returns>
		public string ToPrintStr() {
			string str = "";
			foreach (KeyValuePair<string, object> pair in m_values) {
				if (pair.Value == null) {
					throw new WxPayException("WxPayData内部含有值为null的字段!");
				}
				str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
			}
			return str;
		}

		/// <summary>
		/// 生成签名，详见签名生成算法
		/// </summary>
		/// <returns></returns>
		public string MakeSign(WxPayConfig config) {
			// 转url格式
			string str = ToUrl();
			// 在string后加入API KEY
			str += "&key=" + config.KEY;
			// MD5签名
			var md5 = MD5.Create();
			var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
			var sb = new StringBuilder();
			foreach (byte b in bs) {
				sb.Append(b.ToString("x2"));
			}
			// 所有字符转为大写
			return sb.ToString().ToUpper();
		}

		/// <summary>
		/// 检测签名是否正确
		/// 正确返回true，错误抛异常
		/// </summary>
		/// <returns></returns>
		public bool CheckSign(WxPayConfig config) {
			// 如果没有设置签名，则跳过检测
			if (!IsSet("sign")) {
				throw new WxPayException("WxPayData签名存在但不合法!");
			}
			// 如果设置了签名但是签名为空，则抛异常
			else if (GetValue("sign") == null || GetValue("sign").ToString() == "") {
				throw new WxPayException("WxPayData签名存在但不合法!");
			}
			// 获取接收到的签名
			string return_sign = GetValue("sign").ToString();
			// 在本地计算新的签名
			string cal_sign = MakeSign(config);
			if (cal_sign == return_sign) {
				return true;
			}
			throw new WxPayException("WxPayData签名验证错误!");
		}

		/// <summary>
		/// 获取Dictionary
		/// </summary>
		/// <returns></returns>
		public SortedDictionary<string, object> GetValues() {
			return m_values;
		}
	}
}