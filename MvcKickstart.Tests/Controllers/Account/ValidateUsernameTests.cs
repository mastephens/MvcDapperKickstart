using FizzWare.NBuilder;
using MvcKickstart.Infrastructure;
using MvcKickstart.Models.Users;
using NUnit.Framework;
using Should.Fluent;

namespace MvcKickstart.Tests.Controllers.Account
{
	public class ValidateUsernameTests : ControllerTestBase
	{
		public override void SetupFixture()
		{
			base.SetupFixture();

			using (var session = Store.OpenSession())
			{
				session.Store(Builder<User>.CreateNew()
								  .With(x => x.Id = null)
								  .With(x => x.Username = "TestUser")
								  .With(x => x.IsDeleted = false)
								  .Build());
				session.Store(Builder<User>.CreateNew()
								  .With(x => x.Id = null)
								  .With(x => x.Username = "TestUserDeleted")
								  .With(x => x.IsDeleted = true)
								  .Build());
				session.SaveChanges();
			}
		}

		[Test]
		public void GivenRequest_ReturnsJson()
		{
			var result = Controller.ValidateUsername("TestUser") as JsonNetResult;
			result.Should().Not.Be.Null();
			result.Data.Should().Not.Be.Null();
			result.Data.Should().Be.OfType<bool>();
		}

		[Test]
		public void GivenReservedUsername_ReturnsFalse()
		{
			var result = Controller.ValidateUsername("Admin") as JsonNetResult;
			result.Should().Not.Be.Null();
			((bool) result.Data).Should().Be.False();
		}

		[Test]
		public void GivenExistingUsername_ReturnsFalse()
		{
			var result = Controller.ValidateUsername("TestUser") as JsonNetResult;
			result.Should().Not.Be.Null();
			((bool) result.Data).Should().Be.False();
		}

		[Test]
		public void GivenExistingUsername_IgnoresUsernameCase()
		{
			var result = Controller.ValidateUsername("testuser") as JsonNetResult;
			result.Should().Not.Be.Null();
			((bool) result.Data).Should().Be.False();
		}

		[Test]
		public void GivenExistingDeletedUsername_ReturnsTrue()
		{
			var result = Controller.ValidateUsername("TestUserDeleted") as JsonNetResult;
			result.Should().Not.Be.Null();
			((bool) result.Data).Should().Be.True();
		}

		[Test]
		public void GivenNonExistingUsername_ReturnsTrue()
		{
			var result = Controller.ValidateUsername("nonExistantUser") as JsonNetResult;
			result.Should().Not.Be.Null();
			((bool) result.Data).Should().Be.True();
		}
	}
}
