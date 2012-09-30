using System;

namespace MvcKickstart.Models.Users
{
	public class User
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public string Email { get; set; }
		public DateTime LastActivity { get; set; }

		public bool IsAdmin { get; set; }
		public bool IsDeleted { get; set; }
	}
}