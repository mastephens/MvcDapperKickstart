using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MvcKickstart.ViewModels.Shared
{
	public class RedirectError : Error
	{
		[JsonProperty("redirectUrl", Required = Required.Always)]
		public string RedirectUrl { get; set; }
	}
}