using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Base
{
	public static class KeyWidgetRemoveButton
	{
		private static readonly Color Color = new(1f, 0.4f, 0.4f);

		private const string TextLabel = "×";
		private const string TextTooltip = "Remove this key";
		
        public static bool Draw()
        {
            var colorPrevious = GUI.backgroundColor;
            GUI.backgroundColor = Color;

            var isClicked = KeyWidgetBase.DrawButtonWidthSmall(TextLabel, TextTooltip);

            GUI.backgroundColor = colorPrevious;

            return isClicked;
        }
	}
}
