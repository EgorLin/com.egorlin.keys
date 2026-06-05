using System.Collections.Generic;
using EgorLin.Keys.Owners;
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public class KeyCollectionComponent : KeyCollectionComponentBase
	{
        [SerializeField] public KeyCollection Collection = new();

        public override IEnumerable<IKeyCollectionOwner> GetCollections()
        {
	        yield return Collection;
        }
	}
}
