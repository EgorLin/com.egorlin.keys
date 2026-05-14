using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	public class KeyCollection : KeyCollectionBase
	{
		[SerializeField] private KeyTagValues _keys;

		public List<KeyTag> Keys => _keys.Values;

		public override IEnumerable<KeyTag> GetAllKeys()
		{
			return _keys.Values;
		}

		public override KeyTag GetKeyById(KeyId id)
		{
			foreach (var keyItem in Keys)
			{
				if (keyItem.Id == id)
				{
					return keyItem;
				}
			}
			
			return KeyTag.Empty;
		}

		protected override void InitializeInternal()
		{
			_keys ??= new KeyTagValues() {Values = new List<KeyTag>()};
		}

		protected override KeyTag GetKey(int index)
		{
			return Keys[index];
		}

		protected override void SetKey(KeyTag key, int index)
		{
			_keys.Values[index] = key;
		}

		protected override int GetKeysCount()
		{
			return Keys.Count;
		}
	}
}