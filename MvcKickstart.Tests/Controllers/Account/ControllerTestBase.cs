using FizzWare.NBuilder;
using Moq;
using MvcKickstart.Controllers;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using MvcKickstart.Services;
using MvcKickstart.Tests.Utilities;
using NUnit.Framework;

namespace MvcKickstart.Tests.Controllers.Account
{
	public abstract class ControllerTestBase : TestBase
	{
		public Mock<IUserAuthenticationService> AuthenticationService { get; set; }
		public AccountController Controller { get; set; }
		protected User User { get; private set; }


		[SetUp]
		public override void Setup()
		{
			base.Setup();

			AuthenticationService = new Mock<IUserAuthenticationService>();
			AuthenticationService.Setup(x => x.ReservedUsernames).Returns(new[] { "admin" });

		Controller = new AccountController(Session, Metrics, AuthenticationService.Object);
			ControllerUtilities.SetupControllerContext(Controller);

			User = Builder<User>.CreateNew()
				.With(x => x.Id = null)
				.With(x => x.Password = "password".ToSHAHash())
				.Build();
			Session.Store(User);
			Session.SaveChanges();
		}
	}
}
