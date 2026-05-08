using System.Collections.Generic;

namespace EgorLin.Keys.Owners
{
	public interface IKeyCollectionContainer
	{
		IEnumerable<IKeyCollectionOwner> GetCollections();
	}
}
