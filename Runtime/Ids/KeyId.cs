using System;
using EgorLin.Keys.Utils;

namespace EgorLin.Keys.Ids
{
	[Serializable]
	public struct KeyId : IEquatable<KeyId>
	{
		public int Hash;
		
		public static KeyId Empty => new(0);
		
		public bool IsEmpty => Hash == 0;

		private KeyId(string value)
		{
			Hash = HashUtils.StringToHash32(value);
		}
		
		private KeyId(int hash)
		{
			Hash = hash;
		}

		public bool Equals(KeyId other)
		{
			return Hash == other.Hash;
		}
		
		public override int GetHashCode()
		{
			return Hash;
		}
		
		public override string ToString()
		{
			return Hash.ToString();
		}
		
		public static implicit operator int(KeyId id)
		{
			return id.Hash;
		}

		public static KeyId Create()
		{
			var guid = Guid.NewGuid().ToString();
			
			return new KeyId(guid);
		}
		
		public static KeyId Create(string hashValue)
		{
			return new KeyId(hashValue);
		}
		
		public static KeyId Create(int hashValue)
		{
			return new KeyId(hashValue);
		}
	}
}