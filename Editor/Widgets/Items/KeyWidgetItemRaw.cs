using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemRaw
	{
        public static KeyWidgetItemRawResult Draw(KeyTag tag, KeyId itemId)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            var textTag = tag.Value;
            
            var isRemoveClicked = KeyWidgetRemoveButton.Draw();
            var isRenameClicked = KeyWidgetItemRenameButton.Draw();
            DrawCopyButton(textTag);
            
            KeyWidgetItemTag.Draw(textTag);

            GUILayout.FlexibleSpace();
            
            KeyWidgetItemHash.Draw(itemId.Hash);

            EditorGUILayout.EndHorizontal();
            
            return new KeyWidgetItemRawResult(isRemoveClicked, isRenameClicked);
        }
        
        private static void DrawCopyButton(string tagValue)
        {
	        if (KeyWidgetItemCopyButton.Draw())
	        {
		        EditorGUIUtility.systemCopyBuffer = tagValue;
		        Debug.Log(string.Format(KeyWidgetItemCopyButton.TextDebug, tagValue));
	        }
        }
	}

	public struct KeyWidgetItemRawResult
	{
		public readonly bool IsRemoveClicked;
		public readonly bool IsRenameClicked;

		public KeyWidgetItemRawResult(bool isRemoveClicked, bool isRenameClicked)
		{
			IsRemoveClicked = isRemoveClicked;
			IsRenameClicked = isRenameClicked;
		}
	}
}