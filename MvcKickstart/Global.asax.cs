using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using StackExchange.Profiling;
using StructureMap;

namespace MvcKickstart
{
	public class MvcApplication : HttpApplication
	{
		public MvcApplication()
		{
			AuthenticateRequest += (sender, e) =>
			{
				var app = (HttpApplication) sender;
				if (Request.IsLocal || (app.User != null && app.User.Identity.IsAuthenticated && app.User.Identity.Name == "admin"))
				{
					MiniProfiler.Start();
				}
			};
			EndRequest += (sender, e) =>
			{
				MiniProfiler.Stop();
				ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
			};
		}
		protected void Application_Start()
		{
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());

			AreaRegistration.RegisterAllAreas();

			LoggingConfig.Bootstrap();
			IocConfig.Bootstrap();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			RavenConfig.Bootstrap();
			AutomapperConfig.CreateMappings();
		}

		public override string GetVaryByCustomString(HttpContext context, string custom)
		{
			if (custom == "user")
			{
				if (context.User.Identity.IsAuthenticated)
					return context.User.Identity.Name;

				// Anonymous users should share the same cache. 
				return string.Empty;
			}
			return base.GetVaryByCustomString(context, custom);
		}
	}
}