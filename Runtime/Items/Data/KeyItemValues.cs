using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace EgorLin.Keys.Items.Data
{
	[Serializable]
	[HideLabel]
	[InlineProperty]
	[HideReferenceObjectPicker]
	public class KeyItemValues
	{
        public List<KeyItem> Values;
	}
}
