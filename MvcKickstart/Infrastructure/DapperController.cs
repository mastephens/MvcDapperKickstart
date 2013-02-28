using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using Dapper;
using MvcKickstart.Infrastructure;
using MvcKickstart.Models.Users;
using ServiceStack.CacheAccess;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using MvcKickstart.ViewModels.Shared;
using ServiceStack.Logging;

namespace MvcKickstart.Infrastructure
{
	public abstract class DapperController : Controller
	{
		
	    protected IDbConnection DbConnection { get; private set; }
	    protected ICacheClient CacheClient { get; private set; }
		protected ILog Log { get; private set; }

		public new UserPrincipal User
		{
			get
			{
				return (UserPrincipal) base.User;
			}
		}

		protected DapperController(IDbConnection dbConnection, ICacheClient cacheClient)
		{
		    DbConnection = dbConnection;
		    CacheClient = cacheClient;
			Log = LogManager.GetLogger(GetType());
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			// No need to create a new principal object if it already exists (child actions)
			if (filterContext.HttpContext.User is UserPrincipal)
			{
				base.OnAuthorization(filterContext);
				return;
			}

			User user;
			if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.User.Identity.AuthenticationType == "Forms")
			{
			    var cacheKey = "User-" + filterContext.HttpContext.User.Identity.Name;
			    user = CacheClient.Get<User>(cacheKey);
                if (user == null)
                {
                    var userName = filterContext.HttpContext.User.Identity.Name;
                    user = DbConnection.Query<User>(
                        "SELECT TOP 1 * FROM [User] WHERE IsDeleted = 'false' AND Username = @userName",
                        new {userName}).SingleOrDefault();
                    CacheClient.Add(cacheKey, user,TimeSpan.FromMinutes(1));
                }
			}
			else
			{
				user = new User();
			}
            if (filterContext.HttpContext.User != null)
            {
                filterContext.HttpContext.User = new UserPrincipal(user, filterContext.HttpContext.User.Identity);
                Thread.CurrentPrincipal = filterContext.HttpContext.User;
            }
		    base.OnAuthorization(filterContext);
		}

		protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            ViewBag.AddThisScriptPubId = ConfigurationManager.AppSettings.Get("AddThisProfileId");

			base.Execute(requestContext);
			// If this is an ajax request, clear the tempdata notification.
			if (requestContext.HttpContext.Request.IsAjaxRequest())
			{
				TempData[ViewDataConstants.Notification] = null;
			}
		}

        
		public new JsonNetResult Json(object data)
		{
			return new JsonNetResult { Data = data };
		}

		/// <summary>
		/// Returns the specified error object as json.  Sets the response status code to the ErrorCode value
		/// </summary>
		/// <param name="error">Error to return</param>
		/// <returns></returns>
		public JsonNetResult JsonError(Error error)
		{
			return JsonError(error, error.ErrorCode ?? (int) HttpStatusCode.InternalServerError);
		}
		/// <summary>
		/// Returns the specified error object as json.
		/// </summary>
		/// <param name="error">Error to return</param>
		/// <param name="responseCode">StatusCode to return with the response</param>
		/// <returns></returns>
		public JsonNetResult JsonError(Error error, int responseCode)
		{
			Response.StatusCode = responseCode;
			return new JsonNetResult { Data = error };
		}
	}
}