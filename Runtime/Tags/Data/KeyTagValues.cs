using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace EgorLin.Keys.Tags.Data
{
	[Serializable]
	[HideLabel]
	[InlineProperty]
	[HideReferenceObjectPicker]
	public class KeyTagValues
	{
        public List<KeyTag> Values;
	}
}
