using System;
using EgorLin.Keys.Selectors.Assets;
using Sirenix.OdinInspector;

namespace EgorLin.Keys.Collections.Data
{
	[Serializable]
	public class KeySelectorEntryCollection<T>
	{
		[HideReferenceObjectPicker] public KeySelectorEntry<T>[] Values = Array.Empty<KeySelectorEntry<T>>();
	}
	
	[Serializable]
	public class KeySelectorEntry<T>
	{
		[HideLabel] [HideReferenceObjectPicker] [InlineProperty] public KeySelector Key = new();
		public T Value;
	}
}
