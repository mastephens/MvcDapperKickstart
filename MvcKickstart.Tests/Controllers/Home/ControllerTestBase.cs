using MvcKickstart.Controllers;
using MvcKickstart.Tests.Utilities;
using NUnit.Framework;

namespace MvcKickstart.Tests.Controllers.Home
{
	public abstract class ControllerTestBase : TestBase
	{
		protected HomeController Controller { get; set; }

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			Controller = new HomeController(Session, Metrics);
			ControllerUtilities.SetupControllerContext(Controller);
		}
	}
}
