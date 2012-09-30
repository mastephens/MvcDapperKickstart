using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using MvcKickstart.Areas.Admin.ViewModels.Home;
using MvcKickstart.Infrastructure;
using MvcKickstart.Infrastructure.Attributes;
using Raven.Client;

namespace MvcKickstart.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[Restricted(RequireAdmin = true)]
	public class HomeController : RavenController
    {
		public HomeController(IDocumentSession session, IMetricTracker metrics) : base(session, metrics)
		{
		}

		[GET("", RouteName = "Admin_Home_Index")]
		public ActionResult Index()
		{
			var model = new Index();
			return View(model);
		}

		#region Partials

		[Route("__partial__Menu")]
		public ActionResult Menu()
		{
			return PartialView("_Menu");
		}

		#endregion
	}
}
