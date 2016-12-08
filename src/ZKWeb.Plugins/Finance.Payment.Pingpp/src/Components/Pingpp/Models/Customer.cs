#if !NETCORE
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pingpp.Net;

namespace Pingpp.Models {
	public class Customer : Pingpp {
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("created")]
		public int? Created { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("app")]
		public string App { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, object> Metadata { get; set; }

		[JsonProperty("sources")]
		public CardList Sources { get; set; }

		[JsonProperty("default_source")]
		public string DefaultSource { get; set; }


		private const string BaseUrl = "/v1/customers";
		public static Customer Create(Dictionary<string, object> cusParams) {
			var cus = Requestor.DoRequest(BaseUrl, "POST", cusParams);
			return Mapper<Customer>.MapFromJson(cus);
		}

		public static Customer Retrieve(string cusId) {
			var url = string.Format("{0}/{1}", BaseUrl, cusId);
			var cus = Requestor.DoRequest(url, "GET");
			return Mapper<Customer>.MapFromJson(cus);
		}

		public static CustomerList List(Dictionary<string, object> listParams = null) {
			var url = Requestor.FormatUrl(BaseUrl, Requestor.CreateQuery(listParams));
			var cus = Requestor.DoRequest(url, "GET");
			return Mapper<CustomerList>.MapFromJson(cus);
		}

		public static object Delete(string cusId) {
			var url = string.Format("{0}/{1}", BaseUrl, cusId);
			var cus = Requestor.DoRequest(url, "DELETE");
			return JObject.Parse(cus);
		}

		public static Customer Update(string cusId, Dictionary<string, object> cusParams) {
			var url = string.Format("{0}/{1}", BaseUrl, cusId);
			var cus = Requestor.DoRequest(url, "PUT", cusParams);
			return Mapper<Customer>.MapFromJson(cus);
		}
	}

}
#endif
