using System;
using System.Collections.Generic;
using System.Linq;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeyObjectCollection<T> : KeyCollectionBase
	{
		[SerializeField] private KeyObjectEntryCollection<T> _keys;
		[NonSerialized] private IntHashMap<T> _mapId;
		[NonSerialized] private IntHashMap<T> _mapIdValue;

		public List<KeyObjectEntry<T>> Keys => _keys.Values;
		public IntHashMap<T> MapId => GetMapId();
		public IntHashMap<T> MapIdValue => GetMapIdValue();

		public override KeyTag GetKeyById(KeyId id)
		{
			foreach (var keyTag in Keys)
			{
				if (keyTag.Key.Id == id)
				{
					return keyTag.Key;
				}
			}
			
			return KeyTag.Empty;
		}

		public override KeyTag GetKeyByIdValue(KeyId id)
		{
			foreach (var keyTag in Keys)
			{
				if (keyTag.Key.IdValue== id)
				{
					return keyTag.Key;
				}
			}
			
			return KeyTag.Empty;
		}

		protected override void InitializeInternal()
		{
			_keys ??= new KeyObjectEntryCollection<T>() { Values = new List<KeyObjectEntry<T>>() };
		}

		public override IEnumerable<KeyTag> GetAllKeys()
		{
			return Keys.Select(entry => entry.Key);
		}

		protected override KeyTag GetKey(int index)
		{
			return Keys[index].Key;
		}

		protected override void SetKey(KeyTag key, int index)
		{
			var entryNew = new KeyObjectEntry<T>
			{
				Key = key,
				Value = default
			};
			
			Keys[index] = entryNew;
		}

		protected override int GetKeysCount()
		{
			return Keys.Count;
		}

#if UNITY_EDITOR
		public void AddEntry(string tag)
		{
			Keys.Add(new KeyObjectEntry<T>
			{
				Key = KeyTag.Create(tag),
				Value = default
			});
		}

		public void RemoveEntry(KeyObjectEntry<T> entry)
		{
			Keys.Remove(entry);
		}

		public void ClearEntries()
		{
			Keys.Clear();
		}
#endif

		private IntHashMap<T> GetMapId()
		{
			if (_mapId != null)
			{
				return _mapId;
			}
			
			_mapId = new IntHashMap<T>();

			foreach (var keyValue in _keys.Values)
			{
				_mapId.Set(keyValue.Key.Id, keyValue.Value);
			}
			
			return _mapId;
		}
		
		private IntHashMap<T> GetMapIdValue()
		{
			if (_mapIdValue != null)
			{
				return _mapIdValue;
			}
			
			_mapIdValue = new IntHashMap<T>();

			foreach (var keyValue in _keys.Values)
			{
				_mapIdValue.Set(keyValue.Key.IdValue, keyValue.Value);
			}
			
			return _mapIdValue;
		}
	}
}