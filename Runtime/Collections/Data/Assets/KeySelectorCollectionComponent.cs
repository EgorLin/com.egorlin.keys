using EgorLin.Collections.Unsafe;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeySelectorCollectionComponent<T> : MonoBehaviour
	{
		public KeySelectorCollection<T> Collection = new();
		
		public T GetValue(int id)
		{
			return Collection.ValuesMap.GetValueByKey(id);
		}
	}
}
