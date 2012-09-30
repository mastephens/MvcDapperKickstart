using System;
using Raven.Client;

namespace MvcKickstart.Infrastructure.Extensions
{
	public static class DocumentSessionExtensions
	{
		/// <summary>
		/// An empty class that represents a non caching context. 
		/// </summary>
		public class NonCachingContext : IDisposable
		{
			public void Dispose()
			{
			}
		}

		/// <summary>
		/// Gets the RavenDB caching context if the admin has configured a cache duration; otherwise it will return a non caching context
		/// </summary>
		/// <remarks>From: http://development.msnbc.msn.com/_news/2012/02/23/10462015-raven-db-lessons-learned-caching-contexts?lite</remarks>
		/// <param name="session">RavenDB session</param>
		/// <returns></returns>
		public static IDisposable GetCachingContext(this IDocumentSession session)
		{
			// TODO: Wire up support for CacheSettings in the admin
			if (true)
			{
				return new NonCachingContext();
			}
//			return session.Advanced.DocumentStore.AggressivelyCacheFor(CacheSettings.RavenAggressiveCachingDurationSeconds);
		}
	}
}