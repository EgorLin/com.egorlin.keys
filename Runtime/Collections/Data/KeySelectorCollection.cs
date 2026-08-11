using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EgorLin.Keys.Collections.Data
{
    [Serializable]
    public abstract class KeySelectorCollectionBase
    {
        [SerializeField] internal bool isSpecificCollection;
        [SerializeField] internal Object specificCollection;
 
        internal abstract KeySelectorEntryCollectionBase EntriesBase { get; }
    }
    
	[Serializable]
    public class KeySelectorCollection<T> : KeySelectorCollectionBase
    {
        [SerializeField] private KeySelectorEntryCollection<T> _keys = new();
        [NonSerialized] private Dictionary<int, T> _valuesMap;
 
        public KeySelectorEntry<T>[] Keys => _keys.Values;
        public Dictionary<int, T> ValuesMap => BuildMap();
 
        internal override KeySelectorEntryCollectionBase EntriesBase => _keys;
 
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
