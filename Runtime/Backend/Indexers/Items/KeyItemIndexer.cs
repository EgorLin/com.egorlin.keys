using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;

namespace EgorLin.Keys.Backend.Indexers.Items
{
	public static class KeyItemIndexer
	{
		public static KeyTag GetValue(KeyId keyId)
		{
			var keyCollectionOwner = KeyCollectionOwnerIndexer.Get(keyId);
			var keyItems = keyCollectionOwner.GetAllKeys();

			foreach (var keyItem in keyItems)
			{
				if (keyItem.Id == keyId)
				{
					return keyItem;
				}
			}

            return KeyTag.Empty;
		}
	}
}