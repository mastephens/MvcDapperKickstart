using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MvcKickstart.Areas.Admin.Controllers;
using MvcKickstart.Models.Users;
using MvcKickstart.Tests.Utilities;
using Raven.Client;

namespace MvcKickstart.Tests.Controllers.Admin.Home
{
	public abstract class ControllerTestBase : TestBase
	{
		protected static User User { get; private set; }
		protected HomeController Controller { get; set; }

		protected IDocumentSession Session { get; set; }

		public override void SetupFixture()
		{
			base.SetupFixture();

			User = Builder<User>.CreateNew()
				.With(x => x.Id = null)
				.With(x => x.Username = "admin_" + GetRandom.String(20))
				.With(x => x.IsAdmin = true)
				.Build();

			using (var session = Store.OpenSession())
			{
				session.Store(User);
				session.SaveChanges();
			}
		}
		
		public override void Setup()
		{
			base.Setup();

			Controller = new HomeController(Session, Metrics);
			ControllerUtilities.SetupControllerContext(Controller, User);
		}
	}
}
