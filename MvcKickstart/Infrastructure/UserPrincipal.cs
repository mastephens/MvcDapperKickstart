using System.Security.Principal;
using MvcKickstart.Models.Users;

namespace MvcKickstart.Infrastructure
{
	public class UserPrincipal : IPrincipal
	{
		private readonly User _user;
		public User UserObject { get { return _user; } }

		public UserPrincipal(User user, IIdentity identity)
		{
			_user = user;
			Identity = identity;
		}

		public IIdentity Identity { get; private set; }


		public bool IsInRole(string role)
		{
			// Not really needed in this app
			return true;
		}

		public bool IsAdmin
		{
			get
			{
				return _user != null && _user.IsAdmin;
			}
		}
	}
}