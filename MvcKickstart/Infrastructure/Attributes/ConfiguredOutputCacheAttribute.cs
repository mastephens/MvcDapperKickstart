using System.Web.Mvc;
using DevTrends.MvcDonutCaching;

namespace MvcKickstart.Infrastructure.Attributes
{
	public class ConfiguredOutputCacheAttribute : DonutOutputCacheAttribute
	{
		public ConfiguredOutputCacheAttribute()
		{
			// Set the order relatively high so that we can have other attributes run before this one
			Order = 100;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.Duration = 0;// TODO: CacheSettings.OutputCacheDurationSeconds;
			base.OnActionExecuting(filterContext);
		}
	}
}