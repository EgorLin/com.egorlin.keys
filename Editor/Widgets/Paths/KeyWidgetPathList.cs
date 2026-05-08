using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Paths.Data;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Paths
{
	public static class KeyWidgetPathList
	{
		private const string TextLabelArrow = "▶";
        private static readonly GUILayoutOption OptionArrowWidth = GUILayout.Width(15);

        private static readonly Color ColorNewButton = new(0.4f, 0.8f, 0.4f);

        private const string LabelNewButton = "+";
        private const string TooltipNewButton = "Add new path segment";

        private static readonly GUILayoutOption[] LayoutOptionsNewButton = {
            GUILayout.Width(30),
            GUILayout.Height(22)
        };
        
        public static void DrawPath(KeyCollectionBase collection)
        {
            EditorGUILayout.BeginHorizontal();
            
            for (var indexDepth = 0; indexDepth < collection.Paths.Count; indexDepth++)
            {
                KeyWidgetPathSegment.Draw(collection.Paths, indexDepth);
                
                if (indexDepth < collection.Paths.Count - 1)
                {
                    GUILayout.Label(TextLabelArrow, OptionArrowWidth);
                }
            }

            if (collection.Paths.Count < KeyPathDepth.MaxPathDepth)
            {
                DrawNewButton(collection);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private static void DrawNewButton(KeyCollectionBase collection)
        {
            if (KeyWidgetBase.DrawColoredButton(LabelNewButton, TooltipNewButton, 
                    ColorNewButton, LayoutOptionsNewButton))
            {
                KeyWidgetWindowAddTag.Open(null, collection.AddPath);
            }
        }
	}
}