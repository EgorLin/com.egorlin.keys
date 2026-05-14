using System.Collections.Generic;
using EgorLin.Keys.Paths.Data;
using EgorLin.Keys.Tags.Data;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Paths
{
	public static class KeyWidgetPathHeader
	{
		private const string TextLabelEmptyPath = "📁 Empty Path - Click + to add segments";
		
		private static readonly GUIStyle StyleEmptyPath = EditorStyles.miniLabel;

		private const string TextLabelPath = "📁 {0}";
		
		private const string TextLabelStatsDepth = "Depth: {0}/{1}";
		
		public static readonly GUIStyle StylePath = new(EditorStyles.boldLabel)
		{
			fontSize = 11,
			normal = { textColor = new Color(0.7f, 0.7f, 0.7f) }
		};
		
		public static void Draw(string textFormatedPath, int pathCount)
		{
            EditorGUILayout.BeginHorizontal();
            
            if (pathCount == 0)
            {
                EditorGUILayout.LabelField(TextLabelEmptyPath, StyleEmptyPath);
            }
            else
            {
                EditorGUILayout.LabelField(textFormatedPath, StylePath);
            }
            
            GUILayout.FlexibleSpace();
            
            var labelDepth = string.Format(TextLabelStatsDepth, pathCount.ToString(), KeyPathDepth.MaxPathDepth.ToString());
            GUILayout.Label(labelDepth, EditorStyles.miniLabel);
            
            EditorGUILayout.EndHorizontal();
		}
		
		public static string GetLabelPath(List<KeyTag> pathValues)
		{
			var pathString = "";
			
			for (var index = 0; index < pathValues.Count; index++)
			{
				var value = pathValues[index];

				pathString += value.Value;
				if (index != pathValues.Count - 1)
				{
					pathString += " / ";
				}
			}

			var path = string.Format(TextLabelPath, pathString);
			
			return path;
		}
	}
}