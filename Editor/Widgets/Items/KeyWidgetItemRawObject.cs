using System.Collections.Generic;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.Widgets.Base;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
    public static class KeyWidgetItemRawObject
    {
        private const float ValueFieldWidth = 200f;
        private static readonly GUIContent LabelValue = new("Value");
 
        private static readonly Dictionary<int, bool> FoldoutStates = new();
 
        public static KeyWidgetItemRawResult Draw<T>(
            KeyObjectEntry<T> entry,
            SerializedProperty valueSP)
        {
            var textTag      = entry.Key.Value;
            var isObjectType = typeof(Object).IsAssignableFrom(typeof(T));
 
            if (isObjectType)
            {
                EditorGUILayout.BeginHorizontal();
 
                var isRemoveClicked = KeyWidgetRemoveButton.Draw();
                var isRenameClicked = KeyWidgetItemRenameButton.Draw();
                DrawCopyButton(textTag);
 
                KeyWidgetItemTag.Draw(textTag);
 
                GUILayout.FlexibleSpace();
 
                KeyWidgetItemHash.Draw(entry.Key.Id.Hash);
 
                var valueRect = EditorGUILayout.GetControlRect(
                    GUILayout.Width(ValueFieldWidth),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
 
                if (valueSP != null)
                {
                    EditorGUI.PropertyField(valueRect, valueSP, GUIContent.none);
                }
 
                EditorGUILayout.EndHorizontal();
 
                return new KeyWidgetItemRawResult(isRemoveClicked, isRenameClicked);
            }
            else
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
 
                EditorGUILayout.BeginHorizontal();
 
                var isRemoveClicked = KeyWidgetRemoveButton.Draw();
                var isRenameClicked = KeyWidgetItemRenameButton.Draw();
                DrawCopyButton(textTag);
 
                KeyWidgetItemTag.Draw(textTag);
 
                GUILayout.FlexibleSpace();
 
                KeyWidgetItemHash.Draw(entry.Key.Id.Hash);
 
                EditorGUILayout.EndHorizontal();
 
                if (valueSP != null)
                {
                    DrawValueFoldout(entry.Key.Id.Hash, valueSP);
                }
 
                EditorGUILayout.EndVertical();
 
                return new KeyWidgetItemRawResult(isRemoveClicked, isRenameClicked);
            }
        }
 
        private static void DrawValueFoldout(int id, SerializedProperty valueSP)
        {
            FoldoutStates.TryGetValue(id, out var expanded);
 
            expanded = EditorGUILayout.Foldout(expanded, LabelValue, true);
 
            FoldoutStates[id] = expanded;
 
            if (!expanded)
            {
                return;
            }
 
            EditorGUI.indentLevel++;
 
            var child     = valueSP.Copy();
            var end       = valueSP.GetEndProperty();
            var enterChildren = true;
 
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                EditorGUILayout.PropertyField(child, true);
                enterChildren = false;
            }
 
            EditorGUI.indentLevel--;
        }
 
        private static void DrawCopyButton(string tagValue)
        {
            if (KeyWidgetItemCopyButton.Draw(KeyWidgetItemCopyButton.TextTooltipTag))
            {
                EditorGUIUtility.systemCopyBuffer = tagValue;
                Debug.Log(string.Format(KeyWidgetItemCopyButton.TextDebug, tagValue));
            }
        }
    }
}