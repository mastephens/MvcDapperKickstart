using System.Configuration;
using System.Data;
using ServiceStack.CacheAccess;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StructureMap.Configuration.DSL;

namespace MvcKickstart.Infrastructure
{
	public class IocRegistry : Registry
	{
		public IocRegistry()
		{
			Scan(scan =>
					{
						scan.TheCallingAssembly();
						scan.WithDefaultConventions();
					});

//			For<IDependencyResolver>().Singleton().Use<StructureMapSignalrDependencyResolver>();
//			For<IConnectionManager>().Singleton().Use(GlobalHost.ConnectionManager);

            For<IDbConnection>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() =>
                {
                    var connection = ConnectionFactory.GetOpenConnection();
                    return new ProfiledDbConnection(connection, MiniProfiler.Current);
                });

			For<IMetricTracker>()
				.Singleton()
				.Use(x =>
				     	{
				     		int port;
				     		int.TryParse(ConfigurationManager.AppSettings["Metrics:Port"], out port);
				     		return new MetricTracker(ConfigurationManager.AppSettings["Metrics:Host"], port);
				     	})
				.Named("Metric Tracker");

			For<ICacheClient>()
				.Singleton()
				.Use(x => new MemoryCacheClient());
		}
	}
}
