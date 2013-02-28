using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using FluentScheduler;

namespace MvcKickstart.Infrastructure.Tasks
{
    public class KeepAliveTask : ITask
	{
		public void Execute()
		{
            //HttpWebRequest req = (HttpWebRequest)
            //WebRequest.Create(ConfigurationManager.AppSettings["DomainName"]);
            //req.Method = "GET";

            //req.GetRequestStream();
		}
    }
}