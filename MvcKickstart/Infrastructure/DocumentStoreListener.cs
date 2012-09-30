using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace MvcKickstart.Infrastructure
{
	public class DocumentStoreListener : IDocumentStoreListener
	{
		public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
		{
			/*var entityType = entityInstance.GetType();
			AuditUser user = null;
			var context = HttpContext.Current;
			if (context != null)
			{
				var currentUser = context.User as UserPrincipal;
				if (currentUser != null && currentUser.UserObject != null && !string.IsNullOrEmpty(currentUser.UserObject.Id))
					user = Mapper.Map<AuditUser>(currentUser.UserObject);
			}

			if (original == null || original.Count == 0)
			{
				var createdBy = entityType.GetProperty("CreatedBy");
				if (createdBy != null)
					createdBy.SetValue(entityInstance, user);
				var createdOn = entityType.GetProperty("CreatedOn");
				if (createdOn != null)
					createdOn.SetValue(entityInstance, DateTime.UtcNow);
			}

			var modifiedBy = entityType.GetProperty("ModifiedBy");
			if (modifiedBy != null)
				modifiedBy.SetValue(entityInstance, user);
			var modifiedOn = entityType.GetProperty("ModifiedOn");
			if (modifiedOn != null)
				modifiedOn.SetValue(entityInstance, DateTime.UtcNow);*/
			return true;
		}

		public void AfterStore(string key, object entityInstance, RavenJObject metadata)
		{
		}
	}
}