using EgorLin.Collections.Unsafe;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeySelectorCollectionComponent<T> : SerializedMonoBehaviour
	{
		[SerializeField] public KeySelectorCollection<T> Collection = new();
		
		public T GetValue(int id)
		{
			return Collection.ValuesMap.GetValueByKey(id);
		}
	}
}
