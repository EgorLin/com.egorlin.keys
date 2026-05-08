using EgorLin.Keys.Editor.Widgets.Base;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemHeader
	{
		private const string TextLabelHeader = "🔑 Keys ({0})";
		
		private static readonly GUIStyle StyleHeader = new(EditorStyles.boldLabel)
		{
			fontSize = 12
		};

		private const string TextButtonClearAll = "Clear All";
		private const string TextTooltipClearAll = "Remove all keys";
		
		public static bool Draw(int count)
		{
			EditorGUILayout.BeginHorizontal();

			DrawLabel(count);

			GUILayout.FlexibleSpace();

			var isClicked = DrawClearButton(count);

			EditorGUILayout.EndHorizontal();

			return isClicked;
		}

		private static void DrawLabel(int count)
		{
			var label = string.Format(TextLabelHeader, count.ToString());
			EditorGUILayout.LabelField(label, StyleHeader);
		}

		private static bool DrawClearButton(int count)
		{
			var isClicked = false;

			if (count > 0)
			{
				isClicked = KeyWidgetBase.DrawButtonWidthMiddle(TextButtonClearAll, TextTooltipClearAll);
			}

			return isClicked;
		}
	}
}