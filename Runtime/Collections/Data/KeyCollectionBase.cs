using System;
using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using EgorLin.Keys.Tags.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public abstract class KeyCollectionBase : IKeyCollectionOwner, ISerializationCallbackReceiver
	{
		[SerializeField] protected KeyTagValues path;
		[SerializeField] protected Object owner;

		public List<KeyTag> Paths => path.Values;

		protected KeyCollectionBase()
		{
			Initialize();
		}
		
		public void ValidateKeys()
		{
#if UNITY_EDITOR
			var count = GetKeysCount();
			
			for (int index = 0; index < count; index++)
			{
				var key = GetKey(index);
                
				if (key.Id.IsEmpty)
				{
					key = KeyTag.Create("new");
				
					SetKey(key, index);
				}
			}
#endif
		}

		public void AddPath(string tag)
		{
			var value = KeyTag.Create(tag);
			
			path.Values.Add(value);
		}

		public IEnumerable<KeyTag> GetAllPaths()
		{
			return path.Values;
		}

		public Object GetOwner()
		{
			return owner;
		}

		public bool HasOwner()
		{
			return owner != null;
		}

		public void SetOwner(Object owner)
		{
			this.owner = owner;
		}
		
		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			Initialize();
		}

		private void Initialize()
		{
			path ??= new KeyTagValues { Values = new List<KeyTag>() };
			
			InitializeInternal();
		}

		public abstract IEnumerable<KeyTag> GetAllKeys();
		public abstract KeyTag GetKeyById(KeyId id);
		public abstract KeyTag GetKeyByIdValue(KeyId id);
		protected abstract void InitializeInternal();
		protected abstract KeyTag GetKey(int index);
		protected abstract void SetKey(KeyTag key, int index);
		protected abstract int GetKeysCount();

	}
}