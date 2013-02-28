using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;

namespace MvcKickstart.Infrastructure.Tasks
{
    public class TasksRegistry : Registry
    {
        public TasksRegistry()
		{
            //Schedule<KeepAliveTask>()
            //    .ToRunEvery(15).Minutes();
			
        }
    }
}