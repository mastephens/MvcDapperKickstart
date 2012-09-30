using System.Web.Mvc;
using StructureMap;

namespace MvcKickstart.Infrastructure.Attributes
{
	public class HandlesErrorAttribute : HandleErrorAttribute
	{
		private IMetricTracker Metrics { get; set; }

		public HandlesErrorAttribute()
		{
			Metrics = ObjectFactory.GetInstance<IMetricTracker>();
		}

		public override void OnException(ExceptionContext filterContext)
		{
			Metrics.Increment(Metric.Error_Unhandled);

			base.OnException(filterContext);
		}
	}
}