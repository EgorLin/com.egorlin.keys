using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Owners;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
    [CreateAssetMenu(menuName = "Keys/KeysObject", fileName = "KeysObject", order = 0)]
	public abstract class KeyObjectCollectionConfig<T> : KeyCollectionConfigBase
	{
		public KeyObjectCollection<T> Collection = new();

		public override IEnumerable<IKeyCollectionOwner> GetCollections()
		{
			yield return Collection;
		}
		
		public T GetValue(int id)
		{
			return Collection.MapId.GetValueByKey(id);
		}
	}
}