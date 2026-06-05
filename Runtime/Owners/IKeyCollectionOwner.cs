using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using UnityEngine;

namespace EgorLin.Keys.Owners
{
	public interface IKeyCollectionOwner
	{
		IEnumerable<KeyTag> GetAllPaths();
		IEnumerable<KeyTag> GetAllKeys();
		void ValidateKeys();
		void SetOwner(Object owner);
		Object GetOwner();
		bool HasOwner();
		KeyTag GetKeyById(KeyId id);
		KeyTag GetKeyByIdValue(KeyId id);
	}
}
