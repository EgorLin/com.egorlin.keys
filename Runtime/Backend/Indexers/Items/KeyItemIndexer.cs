using System.Collections.Generic;
using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Pools;
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
	
		public static void FillPathTagByIndex(int index, List<string> itemsToFill)
		{
			var hashSet = PoolHashSet<string>.Spawn();
			
			var collections = KeyCollectionOwnerIndexer.GetAllAssets();

			foreach (var owner in collections)
			{
				if (owner.TryGetPathByIndex(index, out var key))
				{
					hashSet.Add(key.Value);
				}
			}
			
			foreach (var value in hashSet)
			{
				itemsToFill.Add(value);
			}
			
			PoolHashSet<string>.Recycle(hashSet);
		}
	}
}