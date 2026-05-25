using System;
using EgorLin.Collections.Unsafe;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeySelectorCollectionComponent<T> : SerializedMonoBehaviour
	{
		[OdinSerialize] [NonSerialized] public KeySelectorCollection<T> Collection = new();
		
		public T GetValue(int id)
		{
			return Collection.ValuesMap.GetValueByKey(id);
		}
	}
}
