using EgorLin.Keys.Editor.Widgets.Base;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemAddButton
	{
		private static readonly Color Color = new(0.4f, 0.8f, 0.4f);
		
		private const string TextLabel = "+ Add Key";
		private const string TextTooltip = "Add a new key to this collection";

        public static bool Draw()
        {
            var prev = GUI.backgroundColor;
            GUI.backgroundColor = Color;

            var isClicked = KeyWidgetBase.DrawButtonHeightSmall(TextLabel, TextTooltip);

            GUI.backgroundColor = prev;

            return isClicked;
        }
	}
}
