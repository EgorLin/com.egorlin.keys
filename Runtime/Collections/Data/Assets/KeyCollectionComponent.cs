using System;
using System.Collections.Generic;
using EgorLin.Keys.Owners;
using Sirenix.Serialization;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public abstract class KeyCollectionComponent : KeyCollectionComponentBase
	{
        [OdinSerialize] [NonSerialized] public KeyCollection Collection = new();

        public override IEnumerable<IKeyCollectionOwner> GetCollections()
        {
	        yield return Collection;
        }
	}
}
