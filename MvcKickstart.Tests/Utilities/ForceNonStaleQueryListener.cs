using Raven.Client;
using Raven.Client.Listeners;

namespace MvcKickstart.Tests.Utilities
{
	public class ForceNonStaleQueryListener : IDocumentQueryListener
	{
		public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
		{
			customization.WaitForNonStaleResults();
		}
	}
}
