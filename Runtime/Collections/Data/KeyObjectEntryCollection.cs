using System;
using System.Collections.Generic;
using EgorLin.Keys.Items.Data;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeyObjectEntryCollection<T>
	{
		public List<KeyObjectEntry<T>> Values;
	}

	[Serializable]
	public class KeyObjectEntry<T>
	{
		public KeyItem Key;
		public T Value;
	}
}
