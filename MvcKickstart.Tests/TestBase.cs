using System.Web;
using System.Web.Routing;
using Moq;
using MvcKickstart.Infrastructure;
using MvcKickstart.Models.Users;
using MvcKickstart.Tests.Utilities;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using ServiceStack.CacheAccess;

namespace MvcKickstart.Tests
{
	public abstract class TestBase
	{
		public IDocumentStore Store { get; set; }
		public IDocumentSession Session { get; set; }
		public IMetricTracker Metrics { get; set; }
		public Mock<IMetricTracker> MetricsMock { get; set; }
		public ICacheClient Cache { get; set; }

		[TestFixtureSetUp]
		public void SetupFixture()
		{
			HttpContext.Current = null; //This needs to be cleared because EmbeddableDocumentStore will try to set a virtual directory via HttpContext.Current.Request.ApplicationPath, which is null
			Cache = new MemoryCacheClient();
			StructureMap.ObjectFactory.Inject(typeof(ICacheClient), Cache);
			AutomapperConfig.CreateMappings();
			if (RouteTable.Routes == null || RouteTable.Routes.Count == 0)
				RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		[SetUp]
		public virtual void SetUp()
		{
			MetricsMock = new Mock<IMetricTracker>();
			Metrics = MetricsMock.Object;

			HttpContext.Current = null; //This needs to be cleared because EmbeddableDocumentStore will try to set a virtual directory via HttpContext.Current.Request.ApplicationPath, which is null
			Store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
			((DocumentStore) Store).RegisterListener(new ForceNonStaleQueryListener());
			IndexCreation.CreateIndexes(typeof(User).Assembly, Store);
			Session = Store.OpenSession();

			StructureMap.ObjectFactory.Inject(typeof(IDocumentStore), Store);
			StructureMap.ObjectFactory.Inject(typeof(IDocumentSession), Store.OpenSession());
			RavenConfig.Bootstrap();
		}

		[TearDown]
		public virtual void TearDown()
		{
			Store.Dispose();
		}
	}
}