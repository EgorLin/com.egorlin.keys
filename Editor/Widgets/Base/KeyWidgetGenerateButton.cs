using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Base
{
	public class KeyWidgetGenerateButton
	{
		private static readonly Color Color = new(0.5f, 0.5f, 0.7f);

		private const string TextLabel = "💻 Generate Keys";
		
		private static readonly GUILayoutOption OptionHeight = GUILayout.Height(30);
		
		public static bool DrawButton()
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
