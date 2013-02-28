using System;
using System.Data;
using System.Linq;
using System.Reflection;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models.Users;
using StructureMap;
using Dapper;
using Dapper.Contrib.Extensions;

namespace MvcKickstart.App_Start
{
    public class DapperConfig
    {
        public static void Bootstrap()
        {
            var DbConnection = ObjectFactory.GetInstance<IDbConnection>();
            // Create the admin user if there is none
           
            if (!DbConnection.Query<User>("SELECT TOP 1 FROM USER WHERE IsAdmin = 1").Any(x => x.IsAdmin))
            {
                var admin = new User
                {
                    Username = "admin",
                    Password = "admin".ToSHAHash(),
                    IsAdmin = true,
                    LastActivity = DateTime.UtcNow,
                    Email = "notset@yet.com"
                };

                DbConnection.Insert(admin);
            }
        }
    }
}