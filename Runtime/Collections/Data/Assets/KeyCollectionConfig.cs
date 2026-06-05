using System.Collections.Generic;
using EgorLin.Keys.Owners;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeyCollectionConfig : KeyCollectionConfigBase
    {
        public KeyCollection Collection = new();

        public override IEnumerable<IKeyCollectionOwner> GetCollections()
        {
	        yield return Collection;
        }
    }
}