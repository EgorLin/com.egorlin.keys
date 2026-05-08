using System.Collections.Generic;
using EgorLin.Keys.Backend;
using EgorLin.Keys.Base.Commands;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Dialogs;
using EgorLin.Keys.Editor.Widgets.Items;
using EgorLin.Keys.Editor.Widgets.Paths;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Tags.Commands;
using EgorLin.Pools;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    public class KeyCollectionDrawer : OdinValueDrawer<KeyCollection>
    {
        private readonly ModelKeyWidgetPathRoot _modelPath = new();
        private readonly ModelKeyItems<KeyItem> _modelKeys = new((item) => item);
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var collection = ValueEntry.SmartValue;
            
            KeyWidgetInfoBox.Draw();

            if (KeyWidgetSaveButton.DrawSaveButton())
            {
                CommandKeyCollectionSaveAsset.Execute(Property);
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.Refresh();
                    KeysBackend.Rebuild();
                };
            }
            
            KeyWidgetPathRoot.Draw(collection, _modelPath);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (KeyWidgetItemHeader.Draw(collection.Keys.Count))
            {
                Clear(collection.Keys);
            }

            KeyWidgetBase.DrawSpaceSmall();
            
            DrawSearchBar();
            
            KeyWidgetBase.DrawSpaceSmall();

            if (KeyWidgetItemAddButton.Draw())
            {
                OpenAdd(collection.Keys);
            }

            KeyWidgetBase.DrawSpaceSmall();

            if (_modelKeys.IsDirty)
            {
                CommandKeyItemUpdateFilteredItems.Execute(collection.Keys, _modelKeys);

                _modelKeys.SetDirty(false);
            }

            DrawList(collection.Keys);

            EditorGUILayout.EndVertical();
        }
        
        private void DrawSearchBar()
        {
            var textSearch = KeyWidgetItemSearchBar.DrawSearchBar(_modelKeys.Text);

            if (textSearch != _modelKeys.Text)
            {
                _modelKeys.SetDirty(true);
            }

            _modelKeys.SetTextSearch(textSearch);
        }

        private void DrawList(List<KeyItem> keys)
        {
            if (keys.Count == 0)
            {
                var hasSourceItems = keys.Count != 0;
                
                KeyWidgetItemList.DrawEmptyHelpBox(hasSourceItems, _modelKeys.Text);
            }
            else
            {
                var result = KeyWidgetItemList.DrawList(_modelKeys, (keyItem) =>
                {
                    var keyTag = CommandKeyTagGetTag.Execute(keyItem.TagId);
                
                    return KeyWidgetItemRaw.Draw(keyTag, keyItem.Id);
                });
                
                if (result.HasItemToRemove)
                {
                    RemoveItem(keys, _modelKeys.FilteredItems[result.Index]);
                }
            }
        }

        private void Clear(List<KeyItem> keys)
        {
            if (!KeyWidgetDialogClear.Draw(keys.Count))
            {
                return;
            }

            keys.Clear();
            
            _modelKeys.SetDirty(true);
        }

        private void RemoveItem(List<KeyItem> keys, KeyItem key)
        {
            keys.Remove(key);
            
            _modelKeys.SetDirty(true);
        }

        private void OpenAdd(List<KeyItem> keys)
        {
            var lockedTagIds = PoolFastList<KeyId>.Spawn();
            
            foreach (var value in keys)
            {
                lockedTagIds.Add(value.TagId);
            }
            
            KeyWidgetWindowAddTag.Open(lockedTagIds, tagId =>
            {
                AddKey(keys, tagId);
                
                PoolFastList<KeyId>.Recycle(lockedTagIds);
            });
        }
        
        private void AddKey(List<KeyItem> keys, KeyId tagId)
        {
            var keyItem = KeyItem.Create(tagId);
            
            keys.Add(keyItem);
            
            _modelKeys.SetDirty(true);
        }
    }
}