using System;
using EgorLin.Keys.Selectors.Assets;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeySelectorEntryCollection<T>
	{
		public KeySelectorEntry<T>[] Values = Array.Empty<KeySelectorEntry<T>>();
	}
	
	[Serializable]
	public class KeySelectorEntry<T>
	{
		public KeySelector Key = new();
		public T Value;
	}
}
