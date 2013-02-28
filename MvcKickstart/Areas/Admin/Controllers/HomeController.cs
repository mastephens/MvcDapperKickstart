using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using MvcKickstart.Areas.Admin.ViewModels.Home;
using MvcKickstart.Infrastructure;
using MvcKickstart.Infrastructure.Attributes;
using ServiceStack.CacheAccess;

namespace MvcKickstart.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[Restricted(RequireAdmin = true)]
	public class HomeController : DapperController
    {
        public HomeController(IDbConnection dbConnection, ICacheClient cacheClient)
            : base(dbConnection, cacheClient)
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
