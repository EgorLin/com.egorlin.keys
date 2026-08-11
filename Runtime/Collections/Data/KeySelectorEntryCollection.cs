using System;
using EgorLin.Keys.Selectors.Assets;

namespace EgorLin.Keys.Collections.Data
{
    public abstract class KeySelectorEntryCollectionBase
    {
        internal abstract int Count { get; }
    }
    
    [Serializable]
    public class KeySelectorEntryCollection<T> : KeySelectorEntryCollectionBase
    {
        public KeySelectorEntry<T>[] Values = Array.Empty<KeySelectorEntry<T>>();
 
        internal override int Count => Values.Length;
    }
	
	[Serializable]
	public class KeySelectorEntry<T>
	{
		public KeySelector Key = new();
		public T Value;
	}
}
