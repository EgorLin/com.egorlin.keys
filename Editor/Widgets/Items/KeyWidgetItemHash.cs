using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public class KeyWidgetItemHash
	{
		private static readonly GUIStyle Style = new(EditorStyles.miniLabel)
		{
			alignment = TextAnchor.MiddleRight,
			normal = { textColor = Color.gray }
		};

		private static readonly GUILayoutOption OptionWidth = GUILayout.Width(80);

		public static void Draw(int hash)
        {
	        EditorGUILayout.LabelField(hash.ToString(), Style, OptionWidth);
        }
	}
}
