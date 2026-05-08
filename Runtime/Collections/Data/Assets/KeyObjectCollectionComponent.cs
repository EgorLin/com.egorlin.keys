using System;
using System.Collections.Generic;
using EgorLin.Keys.Owners;
using Sirenix.Serialization;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public abstract class KeyObjectCollectionComponent<T> : KeyCollectionComponentBase
	{
		[OdinSerialize] [NonSerialized] public KeyObjectCollection<T> Collection = new();

		public override IEnumerable<IKeyCollectionOwner> GetCollections()
		{
			yield return Collection;
		}
	}
}