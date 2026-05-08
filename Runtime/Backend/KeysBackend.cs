#if UNITY_EDITOR
using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Backend.Indexers.Tags;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using EgorLin.Pools;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Backend
{
	public static class KeysBackend
	{
		[InitializeOnLoadMethod]
		private static void ReloadDomain()
		{
			Rebuild();
		}
		
		public static void Rebuild()
		{
			KeyCollectionOwnerIndexer.Clear();
			KeyTagIndexer.Clear();
			
			var tagIds = PoolHashSet<KeyId>.Spawn();
			
			var guids = AssetDatabase.FindAssets("t:ScriptableObject t:Prefab");

			CheckAssets(guids, tagIds);
			
			KeyTagIndexer.Rebuild(tagIds.Value);
			
			PoolHashSet<KeyId>.Recycle(tagIds);
		}

		private static void CheckAssets(string[] guids, PooledHashSet<KeyId> tagIds)
		{
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var assets = AssetDatabase.LoadAllAssetsAtPath(path);

				foreach (var asset in assets)
				{
					if (asset is IKeyCollectionContainer container)
					{
						if (asset is Component component && PrefabUtility.IsPartOfPrefabInstance(component.gameObject))
						{
							continue;
						}
						
						foreach (var collection in container.GetCollections())
						{
							RegisterCollection(collection, tagIds);
						}
					}
				}
			}
		}

		private static void RegisterCollection(IKeyCollectionOwner collection, PooledHashSet<KeyId> tagIds)
		{
			KeyCollectionOwnerIndexer.RegisterOwner(collection);
			
			foreach (var keyItem in collection.GetAllKeys())
			{
				KeyCollectionOwnerIndexer.Add(keyItem, collection);
				tagIds.Add(keyItem.TagId);
			}

			foreach (var keyItem in collection.GetAllPaths())
			{
				tagIds.Add(keyItem.TagId);
			}
		}
	}
}
#endif
