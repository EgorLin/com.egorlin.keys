using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Base
{
	public class KeyWidgetBase
	{
		private const int SpaceSmall = 5;

		private static readonly GUILayoutOption OptionButtonWidthSmall = GUILayout.Width(25);
		private static readonly GUILayoutOption OptionButtonWidthMiddle = GUILayout.Width(75);
		
		private static readonly GUILayoutOption OptionButtonHeightSmall = GUILayout.Height(25);

		public static void DrawSpaceSmall()
		{
			EditorGUILayout.Space(SpaceSmall);
		}
		
		public static bool DrawButtonWidthSmall(string textLabel, string textTooltip = null)
		{
			return GUILayout.Button(GetContent(textLabel, textTooltip), OptionButtonWidthSmall);
		}
		
		public static bool DrawButtonWidthMiddle(string textLabel, string textTooltip = null)
		{
			return GUILayout.Button(GetContent(textLabel, textTooltip), OptionButtonWidthMiddle);
		}
		
		public static bool DrawButtonHeightSmall(string textLabel, string textTooltip = null)
		{
			return GUILayout.Button(GetContent(textLabel, textTooltip), OptionButtonHeightSmall);
		}
		
		public static bool DrawColoredButton(string label, string tooltip, Color color, GUIStyle style, 
			params GUILayoutOption[] options)
		{
			var prevColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			
			var content = GetContent(label, tooltip);

			var clicked = GUILayout.Button(content, style, options);
			
			GUI.backgroundColor = prevColor;
			
			return clicked;
		}
		
		public static bool DrawColoredButton(string label, string tooltip, Color color, params GUILayoutOption[] options)
		{
			var prevColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			
			var content = GetContent(label, tooltip);
			
			var clicked = GUILayout.Button(content, options);
			
			GUI.backgroundColor = prevColor;
			
			return clicked;
		}

		private static GUIContent GetContent(string label, string tooltip = null)
        {
            return string.IsNullOrEmpty(tooltip) ? new GUIContent(label) : new GUIContent(label, tooltip);
        }
	}
}
