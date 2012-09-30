using MvcKickstart.Controllers;
using MvcKickstart.Tests.Utilities;
using NUnit.Framework;

namespace MvcKickstart.Tests.Controllers.Home
{
	public abstract class ControllerTestBase : TestBase
	{
		protected HomeController Controller { get; set; }

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			Controller = new HomeController(Session, Metrics);
			ControllerUtilities.SetupControllerContext(Controller);
		}
	}
}
