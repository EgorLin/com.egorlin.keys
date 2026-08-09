using System;
using System.Collections.Generic;
using System.Linq;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeyObjectCollection<T> : KeyCollectionBase
	{
		[SerializeField] private KeyObjectEntryCollection<T> _keys;
		[NonSerialized] private Dictionary<int, T> _mapId;
		[NonSerialized] private Dictionary<int, T> _mapIdValue;

		public List<KeyObjectEntry<T>> Keys => _keys.Values;
		public Dictionary<int, T> MapId => GetMapId();
		public Dictionary<int, T> MapIdValue => GetMapIdValue();

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

		private Dictionary<int, T> GetMapId()
		{
			if (_mapId != null)
			{
				return _mapId;
			}
			
			_mapId = new Dictionary<int, T>();

			foreach (var keyValue in _keys.Values)
			{
				_mapId[keyValue.Key.Id] = keyValue.Value;
			}
			
			return _mapId;
		}
		
		private Dictionary<int, T> GetMapIdValue()
		{
			if (_mapIdValue != null)
			{
				return _mapIdValue;
			}
			
			_mapIdValue = new Dictionary<int, T>();

			foreach (var keyValue in _keys.Values)
			{
				_mapIdValue[keyValue.Key.IdValue] = keyValue.Value;
			}
			
			return _mapIdValue;
		}
	}
}