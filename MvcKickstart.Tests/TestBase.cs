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
		protected IDocumentStore Store { get; set; }
		protected IDocumentSession Session { get; set; }
		protected IMetricTracker Metrics { get; set; }
		protected Mock<IMetricTracker> MetricsMock { get; set; }
		protected ICacheClient Cache { get; set; }

		[TestFixtureSetUp]
		public virtual void SetupFixture()
		{
			HttpContext.Current = null; //This needs to be cleared because EmbeddableDocumentStore will try to set a virtual directory via HttpContext.Current.Request.ApplicationPath, which is null
			Store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
			((DocumentStore) Store).RegisterListener(new ForceNonStaleQueryListener());
			IndexCreation.CreateIndexes(typeof(User).Assembly, Store);
			StructureMap.ObjectFactory.Inject(typeof(IDocumentStore), Store);
			RavenConfig.Bootstrap();

			Cache = new MemoryCacheClient();
			StructureMap.ObjectFactory.Inject(typeof(ICacheClient), Cache);
			AutomapperConfig.CreateMappings();
			if (RouteTable.Routes == null || RouteTable.Routes.Count == 0)
				RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		[SetUp]
		public virtual void Setup()
		{
			MetricsMock = new Mock<IMetricTracker>();
			Metrics = MetricsMock.Object;

			Session = Store.OpenSession();
			StructureMap.ObjectFactory.Inject(typeof(IDocumentSession), Session);
		}

		[TearDown]
		public virtual void TearDown()
		{
			Session.Dispose();
		}

		[TestFixtureTearDown]
		public virtual void TearDownFixture()
		{
			Store.Dispose();
		}
	}
}