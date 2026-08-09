using System.Collections.Generic;
using EgorLin.Keys.Owners;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public abstract class KeyObjectCollectionComponent<T> : KeyCollectionComponentBase
	{
		public KeyObjectCollection<T> Collection = new();

		public override IEnumerable<IKeyCollectionOwner> GetCollections()
		{
			yield return Collection;
		}

		public T GetValue(int id)
		{
			return Collection.MapId[id];
		}
	}
}