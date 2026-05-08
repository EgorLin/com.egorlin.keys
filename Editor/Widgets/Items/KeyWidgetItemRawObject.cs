using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.Drawers.Collections;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Commands;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemRawObject
	{
		private const float ValueFieldWidth = 200f;
		
        public static KeyWidgetItemRawResult Draw<T>(KeyObjectEntry<T> entry)
        {
	        var tag = CommandKeyTagGetTag.Execute(entry.Key.TagId);
	        
	        var isInvalid = KeyWidgetItemTag.IsTagInvalid(tag);
	        var textTag = KeyWidgetItemTag.GetTagText(tag, isInvalid);
	        
	        var isObjectType = typeof(Object).IsAssignableFrom(typeof(T));
	        if (isObjectType)
	        {
		        EditorGUILayout.BeginHorizontal();
		        
		        KeyWidgetItemTag.Draw(textTag, isInvalid);
		        
		        GUILayout.FlexibleSpace();
		        
		        KeyWidgetItemHash.Draw(entry.Key.Id.Hash);

		        DrawCopyButton(textTag);
		        
		        var valueRect = EditorGUILayout.GetControlRect(GUILayout.Width(ValueFieldWidth),
			        GUILayout.Height(EditorGUIUtility.singleLineHeight));
		        entry.Value = DrawObjectField(valueRect, entry.Value);
		        
		        var isRemoveClicked = KeyWidgetRemoveButton.Draw();
		        
		        EditorGUILayout.EndHorizontal();
		        
		        return new KeyWidgetItemRawResult(isRemoveClicked);
	        }
	        else
	        {
		        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		        
		        EditorGUILayout.BeginHorizontal();
		        
		        KeyWidgetItemTag.Draw(textTag, isInvalid);
		        GUILayout.FlexibleSpace();
		        
		        KeyWidgetItemHash.Draw(entry.Key.Id.Hash);

		        DrawCopyButton(textTag);
		        
		        var isRemoveClicked = KeyWidgetRemoveButton.Draw();
		        
		        EditorGUILayout.EndHorizontal();
        
		        entry.Value = DrawValueFieldLayout(entry.Value, entry.Key.Id);
		        
		        EditorGUILayout.EndVertical();
		        
		        return new KeyWidgetItemRawResult(isRemoveClicked);
	        }
        }
        
        private static T DrawValueFieldLayout<T>(T current, KeyId entryId)
        {
	        var tree = GetOrCreateTree(current, entryId);
	        ((ObjectWrapper<T>)tree.WeakTargets[0]).Value = current;
    
	        InspectorUtilities.BeginDrawPropertyTree(tree, false);
	        tree.Draw(false);
	        InspectorUtilities.EndDrawPropertyTree(tree);
	        tree.ApplyChanges();
    
	        return ((ObjectWrapper<T>)tree.WeakTargets[0]).Value;
        }
        
        private static T DrawObjectField<T>(Rect rect, T current)
        {
	        return (T)(object)EditorGUI.ObjectField(rect, (Object)(object)current, typeof(T), true);
        }
        
        private static PropertyTree GetOrCreateTree<T>(T value, KeyId entryId)
        {
	        if (KeyCollectionDrawerProperties.TryGet(entryId, out var existing))
	        {
		        return existing;
	        }

	        var wrapper = new ObjectWrapper<T> { Value = value };
	        var tree = PropertyTree.Create(wrapper);
	        
	        KeyCollectionDrawerProperties.Set(entryId, tree);;
	        
	        return tree;
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
	
	internal class ObjectWrapper<T>
	{
		[HideLabel]
		[InlineProperty]
		public T Value;
	}
}