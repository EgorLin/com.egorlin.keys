using System.Collections.Generic;
using EgorLin.Keys.Owners;
using Sirenix.OdinInspector;

namespace EgorLin.Keys.Collections.Data.Assets
{
	public abstract class KeyCollectionConfigBase : SerializedScriptableObject, IKeyCollectionContainer
	{
		public abstract IEnumerable<IKeyCollectionOwner> GetCollections();

#if UNITY_EDITOR
		private void OnEnable()
		{
			foreach (var collection in GetCollections())
			{
				collection.ValidateKeys();

				if (!collection.HasOwner())
				{
					collection.SetOwner(this);
				}
			}
		}
#endif
	}
}
