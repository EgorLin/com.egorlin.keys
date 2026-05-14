using EgorLin.Keys.Tags.Data;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemTag
	{
		private const string TextLabelTag = "🏷 {0}";
		private const string TextLabelInvalid = "INVALID";
		
		private static readonly Color ColorInvalid = Color.red;
		
		private static readonly GUIStyle Style = new(EditorStyles.label)
		{
			fontStyle = FontStyle.Bold
		};
		
		private static readonly GUILayoutOption OptionWidth = GUILayout.Width(200);
		
		public static void Draw(string value)
		{
			var prev = GUI.color;
			GUI.color = prev;

			var text = string.Format(TextLabelTag, value);
			
			EditorGUILayout.LabelField(text, Style, OptionWidth);

			GUI.color = prev;
		}
	}
}
