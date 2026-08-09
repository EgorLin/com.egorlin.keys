using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeySelectorCollectionConfig<T> : ScriptableObject
	{
		public KeySelectorCollection<T> Collection = new();
		
		public T GetValue(int id)
		{
			return Collection.ValuesMap[id];
		}
	}
}
