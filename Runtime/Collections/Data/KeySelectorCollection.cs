using System;
using EgorLin.Collections.Unsafe;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	[HideLabel]
	[HideReferenceObjectPicker]
	[InlineProperty]
	public class KeySelectorCollection<T>
	{
		[HideLabel] [HideReferenceObjectPicker] [InlineProperty] [SerializeField] private KeySelectorEntryCollection<T> _keys = new();
		[NonSerialized] private IntHashMap<T> _valuesMap;
		
		public KeySelectorEntry<T>[] Keys => _keys.Values;
		public IntHashMap<T> ValuesMap => BuildMap();
		
		private IntHashMap<T> BuildMap()
		{
			if (_valuesMap != null)
			{
				return _valuesMap;
			}
			
			_valuesMap = new IntHashMap<T>();

			foreach (var keyValue in _keys.Values)
			{
				_valuesMap.Set(keyValue.Key.ID, keyValue.Value);
			}
			
			return _valuesMap;
		}
	}
}
