#if UNITY_EDITOR
using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
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
			
			var guids = AssetDatabase.FindAssets("t:ScriptableObject t:Prefab");

			CheckAssets(guids);
		}

		private static void CheckAssets(string[] guids)
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
							RegisterCollection(collection);
						}
					}
				}
			}
		}

		private static void RegisterCollection(IKeyCollectionOwner collection)
		{
			KeyCollectionOwnerIndexer.RegisterOwner(collection);
			
			foreach (var keyItem in collection.GetAllKeys())
			{
				KeyCollectionOwnerIndexer.Add(keyItem, collection);
			}
		}
	}
}
#endif