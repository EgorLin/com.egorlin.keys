using System;
using System.Collections.Generic;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeySelectorCollection<T>
	{
		[SerializeField] private KeySelectorEntryCollection<T> _keys = new();
		[NonSerialized] private Dictionary<int, T> _valuesMap;
		
		public KeySelectorEntry<T>[] Keys => _keys.Values;
		public Dictionary<int, T> ValuesMap => BuildMap();
		
		private Dictionary<int, T> BuildMap()
		{
			if (_valuesMap != null)
			{
				return _valuesMap;
			}
			
			_valuesMap = new Dictionary<int, T>();

			foreach (var keyValue in _keys.Values)
			{
				_valuesMap[keyValue.Key.ID] = keyValue.Value;
			}
			
			return _valuesMap;
		}
	}
}
