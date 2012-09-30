using System;
using System.Linq;
using System.Reflection;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using StructureMap;

namespace MvcKickstart
{
	public class RavenConfig
	{
		public static void Bootstrap()
		{
			var store = ObjectFactory.GetInstance<IDocumentStore>();

			// Create any indices that are not currently in raven
			IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);

			// Create the admin user if there is none
			using (var session = store.OpenSession())
			{
				if (!session.Query<User>().Any(x => x.IsAdmin))
				{
					var admin = new User
						            {
							            Username = "admin",
							            Password = "admin".ToSHAHash(),
										IsAdmin = true,
							            LastActivity = DateTime.UtcNow,
							            Email = "notset@yet.com"
						            };
					session.Store(admin);
					session.SaveChanges();
				}
			}
		}
	}
}