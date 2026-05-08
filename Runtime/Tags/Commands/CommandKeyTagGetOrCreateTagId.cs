#if UNITY_EDITOR
using EgorLin.Keys.Backend.Database;
using EgorLin.Keys.Backend.Indexers.Tags;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Utils;

namespace EgorLin.Keys.Tags.Commands
{
	public static class CommandKeyTagGetOrCreateTagId
	{
		public static KeyId Execute(string value)
		{
			var formattedValue = KeyTagUtils.Format(value);
			
			if (KeyTagIndexer.TryGetTag(formattedValue, out var tag))
			{
				return tag.Id;
			}
			
			var database = KeyTagDatabaseProvider.Get();
			
			var tagNew = database.CreateTag(formattedValue);
			
			KeyTagIndexer.AddTag(tagNew);

			return tagNew.Id;
		}
	}
}
#endif