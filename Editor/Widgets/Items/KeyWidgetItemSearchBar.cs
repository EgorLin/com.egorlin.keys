using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemSearchBar
	{
        private const string TextLabelClear = "×";
        private static readonly GUILayoutOption OptionClearWidth = GUILayout.Width(20);
        
        public static string DrawSearchBar(string value)
        {
            EditorGUILayout.BeginHorizontal();

            value = DrawTextField(value);

            value = DrawButton(value);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        private static string DrawTextField(string value)
        {
            var style = GUI.skin.FindStyle("ToolbarSearchTextField") ?? EditorStyles.textField;
            
            value = EditorGUILayout.TextField(value, style);
            
            return value;
        }

        private static string DrawButton(string value)
        {
            var isEmpty = string.IsNullOrEmpty(value);
            
            if (!isEmpty && GUILayout.Button(TextLabelClear, OptionClearWidth))
            {
                value = "";
                GUI.FocusControl(null);
            }

            return value;
        }
    }
}
