using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
	public class KeyValueCollectionEntryDrawer<T> : OdinValueDrawer<KeyObjectCollectionDrawer<T>>
	{
		protected override void DrawPropertyLayout(GUIContent label)
		{
			foreach (var propertyChild in Property.Children)
			{
				propertyChild.Draw();
			}
		}
	}
}
