using System.ComponentModel.DataAnnotations;
using MvcKickstart.Infrastructure.Attributes;

namespace MvcKickstart.ViewModels.Account
{
	public class Register
	{
		[Display(Name = "Email", Description = "Please provide a valid address, we need a way to contact you! We don't share this information.")]
		[Required(ErrorMessage = "{0} is required")]
		[EmailAddress(ErrorMessage = "{0} is not a valid email address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "{0} is required")]
		public string Username { get; set; }

		[Display(Name = "Password", Description = "One number and one uppercase letter required.")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "{0} is required")]
		[StringLength(1000, ErrorMessage = "{0} must be at least {2} characters", MinimumLength = 8)]
		[PasswordStrength]
		public string Password { get; set; }

		[Display(Name = "Confirm Password", Description = "Please enter your password again for good measure.")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "{0} is required")]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string PasswordConfirm { get; set; }

		public string ReturnUrl { get; set; }
	}
}