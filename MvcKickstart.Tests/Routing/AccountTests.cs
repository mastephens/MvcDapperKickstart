using System;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using MvcKickstart.Controllers;
using NUnit.Framework;

namespace MvcKickstart.Tests.Routing
{
	public class AccountTests : RouteTestBase
	{
		[Test]
		public void Index()
		{
			"~/account".ShouldMapTo<AccountController>(x => x.Index());
		}

		[Test]
		public void LoginRoute()
		{
			"~/account/login".ShouldMapTo<AccountController>(x => x.Login((string) null));
		}
		[Test]
		public void LogoutRoute()
		{
			"~/account/logout".ShouldMapTo<AccountController>(x => x.Logout(null));
		}
		[Test]
		public void RegisterRoute()
		{
			"~/account/register".ShouldMapTo<AccountController>(x => x.Register((string) null));
		}
		[Test]
		public void ForgotPassword()
		{
			"~/account/forgot-password".ShouldMapTo<AccountController>(x => x.ForgotPassword());
		}
		[Test]
		public void ResetPassword()
		{
			var guid = Guid.NewGuid();
			("~/account/reset-password/" + guid).ShouldMapTo<AccountController>(x => x.ResetPassword(guid));
		}

	}
}
