using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using MvcKickstart.ViewModels.Shared;
using Raven.Client;
using ServiceStack.Logging;

namespace MvcKickstart.Infrastructure
{
	public abstract class RavenController : Controller
	{
		protected IDocumentSession RavenSession { get; private set; }
		protected IMetricTracker Metrics { get; private set; }
		protected ILog Log { get; private set; }

		public new UserPrincipal User
		{
			get
			{
				return (UserPrincipal) base.User;
			}
		}

		protected RavenController(IDocumentSession connection, IMetricTracker metrics)
		{
			RavenSession = connection;
			Metrics = metrics;
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
			if (filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.User.Identity.AuthenticationType == "Forms")
			{
				using (RavenSession.GetCachingContext())
				{
					user = RavenSession.Query<User>().Customize(x => x.WaitForNonStaleResults()).SingleOrDefault(x => x.Username == filterContext.HttpContext.User.Identity.Name);
				}
			}
			else
			{
				user = new User();
			}

			filterContext.HttpContext.User = new UserPrincipal(user, filterContext.HttpContext.User.Identity);
			Thread.CurrentPrincipal = filterContext.HttpContext.User;
			base.OnAuthorization(filterContext);
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