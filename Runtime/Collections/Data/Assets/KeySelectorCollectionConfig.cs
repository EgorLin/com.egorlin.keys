using EgorLin.Collections.Unsafe;
using Sirenix.OdinInspector;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeySelectorCollectionConfig<T> : SerializedScriptableObject
	{
		public KeySelectorCollection<T> Collection = new();
		
		public T GetValue(int id)
		{
			return Collection.ValuesMap.GetValueByKey(id);
		}
	}
}
