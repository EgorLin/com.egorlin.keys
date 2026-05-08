using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Base
{
	public class KeyWidgetSaveButton
	{
		private static readonly Color Color = new(0.4f, 0.8f, 0.4f);

		private const string TextLabel = "💾 Save";
		
		private static readonly GUILayoutOption OptionHeight = GUILayout.Height(30);
		
		public static bool DrawSaveButton()
		{
			var prevColor = GUI.backgroundColor;
			GUI.backgroundColor = Color;
			
			var clicked = GUILayout.Button(TextLabel, OptionHeight);
			
			GUI.backgroundColor = prevColor;
			
			KeyWidgetBase.DrawSpaceSmall();
			
			return clicked;
		}
	}
}
