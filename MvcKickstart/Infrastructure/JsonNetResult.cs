using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MvcKickstart.Infrastructure
{
	public class JsonNetResult : JsonResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var response = context.HttpContext.Response;

			response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null)
				return;

			var settings = new JsonSerializerSettings
				               {
					               ContractResolver = new CamelCasePropertyNamesContractResolver()
				               };
			var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented, settings);
			response.Write(serializedObject);
		}
	}
}