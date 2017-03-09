using System.Net;
using System.Net.Sockets;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Utils {
	/// <summary>
	/// 网络相关的工具类
	/// TODO: ZKWeb 1.7以后移除这里
	/// </summary>
	public static class NetworkUtils {
		/// <summary>
		/// 获取客户端的Ip地址
		/// Http请求存在时返回远程地址，不存在时返回本机地址
		/// 如果本机在LAN中会返回LAN中的地址，不会返回公网IP
		/// </summary>
		/// <returns></returns>
		public static IPAddress GetClientIpAddress() {
			if (HttpManager.CurrentContextExists) {
				return HttpManager.CurrentContext.Request.RemoteIpAddress;
			} else {
				using (var socket =
					new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)) {
					socket.Connect(new IPEndPoint(IPAddress.Parse("8.8.8.8"), 53));
					return ((IPEndPoint)socket.LocalEndPoint).Address;
				}
			}
		}
	}
}
