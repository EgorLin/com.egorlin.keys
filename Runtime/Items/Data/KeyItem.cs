using System;
using EgorLin.Keys.Ids;
using UnityEngine;

namespace EgorLin.Keys.Items.Data
{
	[Serializable]
	public struct KeyItem : IEquatable<KeyItem>
	{
		[HideInInspector] public KeyId TagId;
		[HideInInspector] public KeyId Id;
		
		public static KeyItem Empty => new(KeyId.Empty, KeyId.Empty);
		
		public bool IsTagEmpty => TagId.IsEmpty;

		private KeyItem(KeyId tagId, KeyId id)
		{
			TagId = tagId;
			Id = id;
		}

		public void SetTag(KeyId tagId)
		{
			TagId = tagId;
		}

		public static KeyItem CreateWithoutId(KeyId tagId)
		{
			return new KeyItem(tagId, KeyId.Empty);
		}

		public static KeyItem Create(KeyId tagId)
		{
			var keyId = KeyId.Create();
			
			return new KeyItem(tagId, keyId);
		}
		
		public static KeyItem Create(KeyId tagId, KeyId id)
		{
			return new KeyItem(tagId, id);
		}

		public bool Equals(KeyItem other)
		{
			return TagId == other.TagId && Id == other.Id;
		}
	}
}
