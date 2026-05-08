#if UNITY_EDITOR
using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Backend.Database;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using EgorLin.Pools;
using UnityEngine;

namespace EgorLin.Keys.Backend.Indexers.Tags
{
	public static class KeyTagIndexer
	{
		private static readonly IntHashMap<KeyTag> Map = new();

		public static void Clear()
		{
			Map.Clear();
			
		}

		public static void Rebuild(HashSet<KeyId> savedIds)
		{
			var itemsToRemove = PoolFastList<KeyTag>.Spawn();
			
			var database = KeyTagDatabaseProvider.Get();
			
			var tags = database.GetTags();

			foreach (var tag in tags)
			{
				if (savedIds.Contains(tag.Id))
				{
					AddTag(tag);
				}
				else
				{
					itemsToRemove.Add(tag);
				}
			}

			foreach (var id in itemsToRemove)
			{
				database.Remove(id);
			}
			
			PoolFastList<KeyTag>.Recycle(itemsToRemove);
		}
		
		public static bool TryGetTag(string value, out KeyTag tag)
		{
			var id = KeyId.Create(value);

			return TryGetTag(id, out tag);
		}

		public static bool TryGetTag(KeyId id, out KeyTag tag)
		{
			if (Map.Has(id))
			{
				tag = Map.GetValueByKey(id);
				return true;
			}

			tag = KeyTag.Empty;
			return false;
		}

		public static void AddTag(KeyTag tag)
		{
			var success = Map.Add(tag.Id, tag, out _);
			
			if (!success)
			{
				var oldTag = Map.GetValueByKey(tag.Id);
				
				Debug.LogError($"[{nameof(KeyTag)}] {tag.Value} has same id {tag.Id.ToString()} as {oldTag.Value}");
			}
		}

		public static void SetTag(KeyTag resultTag)
		{
			Map.Set(resultTag.Id, resultTag, out _);
		}

		public static bool Has(string value)
		{
			var keyTag = KeyTag.Create(value);
			
			return Map.Has(keyTag.Id);
		}
	}
}
#endif