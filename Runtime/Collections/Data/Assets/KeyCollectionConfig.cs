using System;
using System.Collections.Generic;
using EgorLin.Keys.Owners;
using Sirenix.Serialization;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeyCollectionConfig : KeyCollectionConfigBase
    {
        [OdinSerialize] [NonSerialized] public KeyCollection Collection = new();

        public override IEnumerable<IKeyCollectionOwner> GetCollections()
        {
	        yield return Collection;
        }
    }
}