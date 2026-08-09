using System;
using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Backend.Indexers.Collection
{
	public class KeyCollectionOwnerIndexer
	{
		private static readonly Dictionary<int, IKeyCollectionOwner> Map = new();
		private static readonly List<IKeyCollectionOwner> Owners = new();

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
			return Map[keyId];
		}

		public static List<IKeyCollectionOwner> GetAllAssets()
		{
			return Owners;
		}

		public static void Add(KeyTag key, IKeyCollectionOwner config)
		{
			var success = Map.TryAdd(key.Id, config);

			if (!success)
			{
				var oldAsset = Map[key.Id];
		            
				Debug.LogError($"[{nameof(IKeyCollectionOwner)}] {config.GetOwner().name} contains same key id {key.Id.ToString()} as {oldAsset.GetOwner().name}");
			}
		}
	}
}