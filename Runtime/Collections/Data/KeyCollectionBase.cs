#if UNITY_EDITOR
#endif
using System;
using System.Collections.Generic;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Owners;
using EgorLin.Keys.Tags.Commands;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	[HideLabel]
	[HideReferenceObjectPicker]
	[InlineProperty]
	public abstract class KeyCollectionBase : IKeyCollectionOwner, ISerializationCallbackReceiver
	{
		[SerializeField] protected KeyItemValues path;
		[SerializeField] protected Object owner;

		public List<KeyItem> Paths => path.Values;

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
					var tagId = CommandKeyTagGetOrCreateTagId.Execute("new");
                    
					key = KeyItem.Create(tagId);
				}
			}
#endif
		}

		public void AddPath(KeyId tagId)
		{
			var value = KeyItem.CreateWithoutId(tagId);
			
			path.Values.Add(value);
		}

		public IEnumerable<KeyItem> GetAllPaths()
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
			path ??= new KeyItemValues { Values = new List<KeyItem>() };
			
			InitializeInternal();
		}


		public abstract IEnumerable<KeyItem> GetAllKeys();
		public abstract KeyItem GetKeyById(KeyId id);
		protected abstract void InitializeInternal();

		protected abstract KeyItem GetKey(int index);

		protected abstract void SetKey(KeyItem key, int index);

		protected abstract int GetKeysCount();

	}
}