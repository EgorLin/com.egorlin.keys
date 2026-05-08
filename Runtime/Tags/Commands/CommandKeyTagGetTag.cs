#if UNITY_EDITOR
using EgorLin.Keys.Backend.Database;
using EgorLin.Keys.Backend.Indexers.Tags;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using EgorLin.Keys.Utils;

namespace EgorLin.Keys.Tags.Commands
{
	public static class CommandKeyTagGetTag
	{
		public static KeyTag Execute(KeyId id)
		{
			if (KeyTagIndexer.TryGetTag(id, out var tag))
			{
				return tag;
			}

			var database = KeyTagDatabaseProvider.Get();

			return database.GetTag(id);
		}
		
		public static KeyTag Execute(string value)
		{
			var format = KeyTagUtils.Format(value);
			
			var id = KeyTag.Create(format);
			
			if (KeyTagIndexer.TryGetTag(id.Id, out var tag))
			{
				return tag;
			}

			var database = KeyTagDatabaseProvider.Get();
			
			return database.GetTag(id.Id);
		}
	}
}
#endif
