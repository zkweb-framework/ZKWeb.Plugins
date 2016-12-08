#if !NETCORE
using System;
using System.Net;
using Pingpp.Models;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;

namespace Pingpp.Exception {
	public class PingppException : BadRequestException {
		public Error Error { get; set; }

		public PingppException(string message) : base(message) { }

		public PingppException(Error pingppError, string type = null, string message = null)
			: base(message) {
			Error = pingppError;
		}
	}
}
#endif
