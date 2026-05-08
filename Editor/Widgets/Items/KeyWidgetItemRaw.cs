using EgorLin.Keys.Editor.Widgets.Base;
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

            var isInvalid = KeyWidgetItemTag.IsTagInvalid(tag);
            var textTag = KeyWidgetItemTag.GetTagText(tag, isInvalid);
            
            KeyWidgetItemTag.Draw(textTag, isInvalid);

            GUILayout.FlexibleSpace();
            
            KeyWidgetItemHash.Draw(itemId.Hash);

	        DrawCopyButton(textTag);

            var isRemoveClicked = KeyWidgetRemoveButton.Draw();

            EditorGUILayout.EndHorizontal();
            
            return new KeyWidgetItemRawResult(isRemoveClicked);
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

		public KeyWidgetItemRawResult(bool isRemoveClicked)
		{
			IsRemoveClicked = isRemoveClicked;
		}
	}
}