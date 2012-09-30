using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKickstart.Models.Users
{
	public class PasswordRetrieval
	{
		public PasswordRetrieval() { }
		public PasswordRetrieval(User user)
		{
			UserId = user.Id;
		}
		public PasswordRetrieval(User user, Guid token) : this(user)
		{
			Token = token;
		}

		public string Id { get; set; }

		public Guid Token { get; set; }
		public string UserId { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}