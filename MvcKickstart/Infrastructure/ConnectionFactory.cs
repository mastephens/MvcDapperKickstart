using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcKickstart.Infrastructure
{
    
        public class ConnectionFactory
        {
            public static DbConnection GetOpenConnection()
            {

                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                connection.Open();
                return connection;
            }
        }
    }