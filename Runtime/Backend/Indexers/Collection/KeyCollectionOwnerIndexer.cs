using System;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Owners;
using UnityEngine;

namespace EgorLin.Keys.Backend.Indexers.Collection
{
	public class KeyCollectionOwnerIndexer
	{
		private static readonly IntHashMap<IKeyCollectionOwner> Map = new();
		private static readonly FastList<IKeyCollectionOwner> Owners = new();

		public static void Clear()
		{
			Map.Clear();
			Owners.Clear();
		}

		public static void RegisterOwner(IKeyCollectionOwner owner)
		{
			Owners.Add(owner);
		}

		public static IKeyCollectionOwner Get(KeyId keyId)
		{
			return Map.GetValueByKey(keyId);
		}

		public static ReadOnlySpan<IKeyCollectionOwner> GetAllAssets()
		{
			return Owners.AsReadOnlySpan();
		}

		public static void Add(KeyItem key, IKeyCollectionOwner config)
		{
			var success = Map.Add(key.Id, config, out _);

			if (!success)
			{
				var oldAsset = Map.GetValueByKey(key.Id);
		            
				Debug.LogError($"[{nameof(IKeyCollectionOwner)}] {config.GetOwner().name} contains same key id {key.Id.ToString()} as {oldAsset.GetOwner().name}");
			}
		}
	}
}