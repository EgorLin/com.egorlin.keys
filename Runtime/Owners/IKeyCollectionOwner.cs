using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using UnityEngine;

namespace EgorLin.Keys.Owners
{
	public interface IKeyCollectionOwner
	{
		IEnumerable<KeyItem> GetAllPaths();
		IEnumerable<KeyItem> GetAllKeys();
		void ValidateKeys();
		void SetOwner(Object owner);
		Object GetOwner();
		bool HasOwner();
		KeyItem GetKeyById(KeyId id);
	}
}
