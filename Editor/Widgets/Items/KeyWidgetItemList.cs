using System;
using EgorLin.Keys.Base.Models;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemList
	{
        private const string MessageNoKeys = "No keys yet. Click + Add Key to create one.";
        private const string MessageNoMatch = "No keys match '{0}'";

        private static readonly GUILayoutOption OptionScrollHeight = GUILayout.MaxHeight(300f);

        public static KeyWidgetItemListResult DrawList<T>(ModelKeyItems<T> model, Func<T, KeyWidgetItemRawResult> drawItem)
        {
            var isObjectType = typeof(Object).IsAssignableFrom(typeof(T));
            
            if (isObjectType)
            {
                var scroll = EditorGUILayout.BeginScrollView(model.ScrollPosition, OptionScrollHeight);
                
                model.SetScrollPosition(scroll);

                var result = DrawItems(model, drawItem);

                EditorGUILayout.EndScrollView();

                return result;
            }

            return DrawItems(model, drawItem);
        }

        public static void DrawEmptyHelpBox(bool hasAny, string text)
        {
            var message = !hasAny ? MessageNoKeys : string.Format(MessageNoMatch, text);
            
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }

        private static KeyWidgetItemListResult DrawItems<T>(ModelKeyItems<T> model, Func<T, KeyWidgetItemRawResult> drawItem)
        {
            var result = new KeyWidgetItemListResult();

            var index = 0;
            foreach (var item in model.FilteredItems)
            {
                var rawResult = drawItem.Invoke(item);
                
                if (rawResult.IsRemoveClicked)
                {
                    result = new KeyWidgetItemListResult(true, index);
                }

                index += 1;
            }

            return result;
        }
    }

    public struct KeyWidgetItemListResult
    {
        public readonly bool HasItemToRemove;
        public readonly int Index;

        public KeyWidgetItemListResult(bool hasItemToRemove, int index)
        {
            HasItemToRemove = hasItemToRemove;
            Index = index;
        }
    }
}
