using System;
using EgorLin.Keys.Ids;

namespace EgorLin.Keys.Tags.Data
{
	[Serializable]
	public struct KeyTag : IEquatable<KeyTag>
	{
		public string Value;
		public KeyId Id;
		
		public static KeyTag Empty => new(KeyId.Empty);
		
		private KeyTag(KeyId id)
		{
			Value = string.Empty;
			Id = id;
		}

		private KeyTag(string value, KeyId id)
		{
			Value = value;
			Id = id;
		}

		public bool IsEmpty()
		{
			return Id.IsEmpty || string.IsNullOrEmpty(Value);
		}

		public bool Equals(KeyTag other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static KeyTag CreateEmpty()
		{
			return new KeyTag();
		}

		public static KeyTag CreateWithEmptyValue(KeyId id)
		{
			return new KeyTag(string.Empty, id);
		}

		public static KeyTag Create(string tag)
		{
			var id = KeyId.Create(tag);
			return new KeyTag(tag, id);
		}
	}
}
