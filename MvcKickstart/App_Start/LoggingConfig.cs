using System.Web;
using System.Web.Mvc;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;

namespace MvcKickstart
{
	public class LoggingConfig
	{
		public static void Bootstrap()
		{
			LogManager.LogFactory = new Log4NetFactory(true);
		}
	}
}