using System;
using EgorLin.Keys.Ids;

namespace EgorLin.Keys.Tags.Data
{
	[Serializable]
	public struct KeyTag : IEquatable<KeyTag>
	{
		public string Value;
		
		public KeyId IdValue;
		public KeyId Id;
		
		public static KeyTag Empty => new(KeyId.Empty);
		
		private KeyTag(KeyId id)
		{
			Value = string.Empty;
			Id = id;
			IdValue = KeyId.Empty;
		}

		private KeyTag(string value, KeyId id)
		{
			Value = value;
			Id = id;
			IdValue = KeyId.Create(value);
		}

		public bool IsEmpty()
		{
			return Id.IsEmpty && IdValue.IsEmpty;
		}

		public bool Equals(KeyTag other)
		{
			return Id == other.Id && IdValue == other.IdValue;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static KeyTag CreateEmpty()
		{
			return Empty;
		}

		public static KeyTag CreateWithEmptyValue(KeyId id)
		{
			return new KeyTag(string.Empty, id);
		}

		public static KeyTag Create(string value)
		{
			var id = KeyId.Create();
			return new KeyTag(value, id);
		}
	}
}
