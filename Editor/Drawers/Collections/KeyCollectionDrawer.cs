using System.Collections.Generic;
using EgorLin.Keys.Backend;
using EgorLin.Keys.Base.Commands;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.CodeGeneration;
using EgorLin.Keys.Editor.Drawers.Utils;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Dialogs;
using EgorLin.Keys.Editor.Widgets.Items;
using EgorLin.Keys.Editor.Widgets.Paths;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Tags.Data;
using EgorLin.Pools;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    [CustomPropertyDrawer(typeof(KeyCollection))]
    public class KeyCollectionDrawer : PropertyDrawer
    {
        private readonly Dictionary<int, DrawerState> _stateMap = new();
 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawLayout(property);
        }
 
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0f;
 
        private DrawerState GetState(SerializedProperty property)
        {
            var id = property.serializedObject.GetHashCode();
 
            if (!_stateMap.TryGetValue(id, out var state))
            {
                state = new DrawerState();
                _stateMap[id] = state;
            }
 
            return state;
        }
 
        private void DrawLayout(SerializedProperty property)
        {
            var state = GetState(property);
 
            var collection = GetCollection(property);
 
            if (collection == null)
            {
                EditorGUILayout.HelpBox("Could not resolve KeyCollection.", MessageType.Error);
                return;
            }
 
            KeyWidgetInfoBox.Draw();
 
            if (KeyWidgetSaveButton.DrawSaveButton(state.IsSaveDirty))
            {
                var owner = property.serializedObject.targetObject;
                CommandKeyCollectionSaveAsset.Execute(owner);
 
                EditorApplication.delayCall += () =>
                {
                    state.IsSaveDirty = false;
                    AssetDatabase.Refresh();
                    KeysBackend.Rebuild();
                };
            }
 
            if (KeyWidgetGenerateButton.DrawButton())
            {
                KeyCollectionCodeGenerator.Generate();
            }
 
            KeyWidgetPathRoot.Draw(collection, state.ModelPath, () => SetDirty(state));
 
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
 
            if (KeyWidgetItemHeader.Draw(collection.Keys.Count))
            {
                Clear(collection.Keys, state);
            }
 
            KeyWidgetBase.DrawSpaceSmall();
            DrawSearchBar(collection.Keys, state);
            KeyWidgetBase.DrawSpaceSmall();
 
            if (KeyWidgetItemAddButton.Draw())
            {
                OpenAdd(collection.Keys, state);
            }
 
            KeyWidgetBase.DrawSpaceSmall();
 
            if (state.ModelKeys.IsDirty)
            {
                CommandKeyItemUpdateFilteredItems.Execute(collection.Keys, state.ModelKeys);
                state.ModelKeys.SetDirty(false);
            }
 
            DrawList(collection.Keys, state);
 
            EditorGUILayout.EndVertical();
        }
 
        private void DrawSearchBar(List<KeyTag> keys, DrawerState state)
        {
            var textSearch = KeyWidgetItemSearchBar.DrawSearchBar(state.ModelKeys.Text);
 
            if (textSearch != state.ModelKeys.Text)
            {
                SetDirty(state);
            }
 
            state.ModelKeys.SetTextSearch(textSearch);
        }
 
        private void DrawList(List<KeyTag> keys, DrawerState state)
        {
            if (keys.Count == 0)
            {
                var hasSourceItems = keys.Count != 0;
                KeyWidgetItemList.DrawEmptyHelpBox(hasSourceItems, state.ModelKeys.Text);
            }
            else
            {
                var result = KeyWidgetItemList.DrawList(
                    state.ModelKeys,
                    keyItem => KeyWidgetItemRaw.Draw(keyItem, keyItem.Id));
 
                if (result.HasItemToRemove)
                {
                    RemoveItem(keys, state.ModelKeys.FilteredItems[result.Index], state);
                }
 
                if (result.HasItemToRename)
                {
                    Rename(keys, state.ModelKeys.FilteredItems[result.Index], state);
                }
            }
        }
 
        private void Clear(List<KeyTag> keys, DrawerState state)
        {
            if (!KeyWidgetDialogClear.Draw(keys.Count))
            {
                return;
            }
 
            keys.Clear();
            SetDirty(state);
        }
 
        private void RemoveItem(List<KeyTag> keys, KeyTag key, DrawerState state)
        {
            keys.Remove(key);
            SetDirty(state);
        }
 
        private void Rename(List<KeyTag> keys, KeyTag key, DrawerState state)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
 
            foreach (var value in keys)
            {
                lockedTagIds.Add(value.Value);
            }
 
            KeyWidgetWindowAddTag.Open(lockedTagIds, true, tag =>
            {
                RenameInList(keys, key, tag);
                SetDirty(state);
                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }
 
        private static void RenameInList(List<KeyTag> keys, KeyTag key, string tag)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                var k = keys[i];
 
                if (k.Id == key.Id)
                {
                    key.Value = tag;
                    keys[i] = key;
                    break;
                }
            }
        }
 
        private void OpenAdd(List<KeyTag> keys, DrawerState state)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
 
            foreach (var value in keys)
            {
                lockedTagIds.Add(value.Value);
            }
 
            KeyWidgetWindowAddTag.Open(lockedTagIds, true, tagId =>
            {
                AddKey(keys, tagId, state);
                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }
 
        private void AddKey(List<KeyTag> keys, string tag, DrawerState state)
        {
            var keyItem = KeyTag.Create(tag);
            keys.Add(keyItem);
            SetDirty(state);
        }
 
        private static void SetDirty(DrawerState state)
        {
            state.ModelKeys.SetDirty(true);
            state.IsSaveDirty = true;
        }
 
        private static KeyCollection GetCollection(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
 
            var fieldInfo = ReflectionUtils.GetFieldInfo(target.GetType(), property.propertyPath);
 
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(target) as KeyCollection;
            }
 
            return null;
        }
 
        private sealed class DrawerState
        {
            public readonly ModelKeyWidgetPathRoot ModelPath = new();
            public readonly ModelKeyItems<KeyTag> ModelKeys = new(item => item);
            public bool IsSaveDirty;
        }
    }
}