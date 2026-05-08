using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	public class KeyCollection : KeyCollectionBase
	{
		[SerializeField] private KeyItemValues _keys;

		public List<KeyItem> Keys => _keys.Values;

		public override IEnumerable<KeyItem> GetAllKeys()
		{
			return _keys.Values;
		}

		public override KeyItem GetKeyById(KeyId id)
		{
			foreach (var keyItem in Keys)
			{
				if (keyItem.Id == id)
				{
					return keyItem;
				}
			}
			
			return KeyItem.Empty;
		}

		protected override void InitializeInternal()
		{
			_keys ??= new KeyItemValues() {Values = new List<KeyItem>()};
		}

		protected override KeyItem GetKey(int index)
		{
			return Keys[index];
		}

		protected override void SetKey(KeyItem key, int index)
		{
			_keys.Values[index] = key;
		}

		protected override int GetKeysCount()
		{
			return Keys.Count;
		}
	}
}