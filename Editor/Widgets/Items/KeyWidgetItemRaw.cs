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
            
	        if (KeyWidgetItemCopyButton.Draw(KeyWidgetItemCopyButton.TextTooltipTag))
	        {
		        EditorGUIUtility.systemCopyBuffer = textTag;
		        Debug.Log(string.Format(KeyWidgetItemCopyButton.TextDebug, textTag));
	        }
	        if (KeyWidgetItemCopyButton.Draw(KeyWidgetItemCopyButton.TextTooltipId))
	        {
		        EditorGUIUtility.systemCopyBuffer = tag.Id.ToString();
		        Debug.Log(string.Format(KeyWidgetItemCopyButton.TextDebug, textTag));
	        }
            
            KeyWidgetItemTag.Draw(textTag);

            GUILayout.FlexibleSpace();
            
            KeyWidgetItemHash.Draw(itemId.Hash);

            EditorGUILayout.EndHorizontal();
            
            return new KeyWidgetItemRawResult(isRemoveClicked, isRenameClicked);
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