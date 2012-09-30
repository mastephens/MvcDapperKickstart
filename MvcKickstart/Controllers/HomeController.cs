using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using MvcKickstart.Infrastructure;
using MvcKickstart.Infrastructure.Attributes;
using MvcKickstart.ViewModels.Home;
using Raven.Client;
using StackExchange.Profiling;

namespace MvcKickstart.Controllers
{
	public class HomeController : RavenController
	{
		public HomeController(IDocumentSession session, IMetricTracker metrics) : base(session, metrics)
		{
		}

		[GET("", RouteName = "Home_Index")]
		[ConfiguredOutputCache]
		public ActionResult Index()
		{
			var model = new Index();
			return View(model);
		}

		#region Partials

		[Route("__partial__Home_Profiler")]
		public ActionResult Profiler()
		{
#if !DEBUG
			if (User.IsAdmin)
			{
#endif
				return Content(MiniProfiler.RenderIncludes().ToHtmlString());
#if !DEBUG
			}
#endif
			return new EmptyResult();
		}

		[Route("__partial__Home_Notification")]
		public ActionResult Notification()
		{
			return PartialView("_Notification");
		}

		[Route("__partial__Home_UsernameOrLogin")]
		[ConfiguredOutputCache(VaryByCustom = "user")]
		public ActionResult UsernameOrLogin()
		{
			// This route could probably also go in a UsersController if it exists
			return PartialView("_UsernameOrLogin");
		}


		#endregion
	}
}