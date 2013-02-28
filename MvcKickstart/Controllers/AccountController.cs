using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;
using Dapper;
using Dapper.Contrib.Extensions;
using MvcKickstart.Infrastructure;
using MvcKickstart.Infrastructure.Attributes;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using MvcKickstart.Services;
using MvcKickstart.ViewModels.Account;
using ServiceStack.CacheAccess;

namespace MvcKickstart.Controllers
{
    public class AccountController : DapperController
    {
        private readonly IUserAuthenticationService _authenticationService;

        public AccountController(IUserAuthenticationService authenticationService, IDbConnection dbConnection, ICacheClient cacheClient)
            : base(dbConnection, cacheClient)
        {
            _authenticationService = authenticationService;
        }

		[GET("account", RouteName = "Account_Index")]
		[ConfiguredOutputCache]
		public ActionResult Index()
		{
			return Redirect(Url.Home().Index());
		}

		[GET("account/login", RouteName = "Account_Login")]
		[ConfiguredOutputCache(VaryByParam = "returnUrl", VaryByCustom = VaryByCustom.UserIsAuthenticated)]
		public ActionResult Login(string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new Login {ReturnUrl = returnUrl};
			return View(model);
		}

		[POST("account/login")]
		public ActionResult Login(Login model)
		{
			if (ModelState.IsValid)
			{
                var userName = model.Username;
                var password = model.Password.ToSHAHash();
                var user = DbConnection.Query<User>(
                        "SELECT TOP 1 * FROM [User] WHERE IsDeleted = 'false' AND Username = @userName AND Password = @password",
                        new { userName, password }).SingleOrDefault();
                if (user != null)
				{
					_authenticationService.SetLoginCookie(user, model.RememberMe);
					
					if (Url.IsLocalUrl(model.ReturnUrl))
						return Redirect(model.ReturnUrl);
					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("InvalidCredentials", string.Format("The user name or password provided is incorrect. Did you <a href='{0}'>forget your password?</a>", Url.Account().ForgotPassword()));
			}
			

			// If we got this far, something failed, redisplay form
			model.Password = null; //clear the password so they have to re-enter it
			return View(model);
		}

		[Restricted]
		[GET("account/logout", RouteName = "Account_Logout")]
		public ActionResult Logout(string returnUrl)
		{
			_authenticationService.Logout();
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);
			return RedirectToAction("Index", "Home");
		}

        #region Register

        [GET("account/register", RouteName = "Account_Register")]
        [ConfiguredOutputCache(VaryByParam = "returnUrl")]
        public ActionResult Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new Register { ReturnUrl = returnUrl });
        }

        [POST("account/register")]
        public ActionResult Register(Register model)
        {
            if (_authenticationService.ReservedUsernames.Any(x => model.Username.Equals(x, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Username", "Username is unavailable");
            }
            else
            {

                var userName = model.Username;
                var existingUsername = DbConnection.Query<User>(
                    "SELECT TOP 1 * FROM User WHERE IsDeleted = 'false' AND Username = @userName",
                    new { userName }).SingleOrDefault();
                if (existingUsername != null)
                {
                    ModelState.AddModelError("Duplicate username", "Username is already in use");
                }
                else
                {
                    var email = model.Email;
                    var existingEmail = DbConnection.Query<User>(
                    "SELECT TOP 1 * FROM User WHERE IsDeleted = 'false' AND Email = @email",
                    new { email }).SingleOrDefault();
                    if (existingEmail != null)
                    {
                        ModelState.AddModelError("Duplicate email", "A user with that email exists");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var newUser = Mapper.Map<User>(model);
                newUser.LastActivity = DateTime.UtcNow;
                newUser.Password = model.Password.ToSHAHash();

                DbConnection.Insert(newUser);

                //Metrics.Increment(Metric.Users_Register);
                _authenticationService.SetLoginCookie(newUser, true);

                // TODO: Send welcome email?

                return View("RegisterConfirmation", model);
            }

            // If we got this far, something failed, redisplay form
            model.Password = null; //clear the password so they have to re-enter it
            return View(model);
        }

        [POST("account/validate-username/{username}", RouteName = "Account_ValidateUsername")]
        [ConfiguredOutputCache(VaryByParam = "username")]
        public JsonResult ValidateUsername(string username)
        {
            if (_authenticationService.ReservedUsernames.Any(x => username.Equals(x, StringComparison.OrdinalIgnoreCase)))
                return Json(false);

            var user = DbConnection.Query<User>(
                       "SELECT TOP 1 * FROM User WHERE IsDeleted = 'false' AND Username = @username",
                       new { username }).SingleOrDefault();
            return Json(user == null);
        }
        #endregion

        #region Forgot Password
        [GET("account/forgot-password", RouteName = "Account_ForgotPassword")]
        [ConfiguredOutputCache]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [POST("account/forgot-password")]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var email = model.Email;
                var user = DbConnection.Query<User>(
                       "SELECT TOP 1 * FROM User WHERE IsDeleted = 'false' AND Email = @email",
                       new { email }).SingleOrDefault();

                //get user by email address



                //if no matching user, error
                if (user == null)
                {
                    ModelState.AddModelError("Invalid User Email", "A user could not be found with that email address");
                    return View(model);
                }

                // Create token and send email
                var token = new PasswordRetrieval(user, Guid.NewGuid());
                DbConnection.Insert(token);
                //Metrics.Increment(Metric.Users_SendPasswordResetEmail);

                // TODO: Send email with password token
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [GET("account/reset-password/{token}", RouteName = "Account_ResetPassword")]
        public ActionResult ResetPassword(Guid token)
        {
            var model = new ResetPassword { Token = token };

            var tokenData = DbConnection.Query<PasswordRetrieval>(
                      "SELECT TOP 1 * FROM PasswordRetrieval WHERE Token = @token",
                      new { token }).SingleOrDefault();


            model.Data = tokenData;

            if (model.Data == null)
                return Redirect(Url.Home().Index());

            return View(model);
        }

        [POST("account/reset-password/{token}")]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {

                model.Data = DbConnection.Query<PasswordRetrieval>(
                  "SELECT TOP 1 * FROM PasswordRetrieval WHERE Token = @token",
                  new { model.Token }).SingleOrDefault();

                if (model.Data == null)
                    return Redirect(Url.Home().Index());

                User.UserObject.Password = model.Password.ToSHAHash();
                DbConnection.Update(User);
                DbConnection.Delete(model.Data);

                //Metrics.Increment(Metric.Users_ResetPassword);
                //}
                //show confirmation
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
        #endregion
	}
}
