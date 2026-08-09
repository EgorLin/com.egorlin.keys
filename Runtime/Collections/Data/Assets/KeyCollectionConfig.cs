using System.Collections.Generic;
using EgorLin.Keys.Owners;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
    [CreateAssetMenu(menuName = "Keys/Keys", fileName = "Keys", order = 0)]
	public class KeyCollectionConfig : KeyCollectionConfigBase
    {
        public KeyCollection Collection = new();

        public override IEnumerable<IKeyCollectionOwner> GetCollections()
        {
	        yield return Collection;
        }
    }
}