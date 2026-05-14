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
using EgorLin.Pools;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    public class KeyObjectCollectionDrawer<T> : OdinValueDrawer<KeyObjectCollection<T>>
    {
        private readonly ModelKeyWidgetPathRoot _modelPath = new();
        private readonly ModelKeyItems<KeyObjectEntry<T>> _modelKeys = new((entry) => entry.Key);

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
                Clear(collection);
            }

            KeyWidgetBase.DrawSpaceSmall();
            
            DrawSearchBar();
            
            KeyWidgetBase.DrawSpaceSmall();

            if (KeyWidgetItemAddButton.Draw())
            {
                OpenAdd(collection);
            }

            KeyWidgetBase.DrawSpaceSmall();

            if (_modelKeys.IsDirty)
            {
                CommandKeyItemUpdateFilteredItems.Execute(collection.Keys, _modelKeys);

                _modelKeys.SetDirty(false);
            }

            DrawList(collection);

            EditorGUILayout.EndVertical();
        }

        private void DrawList(KeyObjectCollection<T> collection)
        {
            if (collection.Keys.Count == 0)
            {
                var hasSourceItems = collection.Keys.Count != 0;
                
                KeyWidgetItemList.DrawEmptyHelpBox(hasSourceItems, _modelKeys.Text);
            }
            else
            {
                var result = KeyWidgetItemList.DrawList(_modelKeys, KeyWidgetItemRawObject.Draw);
                
                if (result.HasItemToRemove)
                {
                    Remove(collection, _modelKeys.FilteredItems[result.Index]);
                }
            }
        }

        private void OpenAdd(KeyObjectCollection<T> collection)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
            
            foreach (var value in collection.Keys)
            {
                lockedTagIds.Add(value.Key.Value);
            }

            KeyWidgetWindowAddTag.Open(lockedTagIds, tagId =>
            {
                AddKey(collection, tagId);
                
                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }
        
        private void AddKey(KeyObjectCollection<T> collection, string tag)
        {
            collection.AddEntry(tag);
            
            _modelKeys.SetDirty(true);
        }

        private void Clear(KeyObjectCollection<T> collection)
        {
            if (!KeyWidgetDialogClear.Draw(collection.Keys.Count))
            {
                return;
            }

            foreach (var entry in collection.Keys)
            {
                KeyCollectionDrawerProperties.Remove(entry.Key.Id);
            }

            collection.ClearEntries();
            
            _modelKeys.SetDirty(true);
        }

        private void Remove(KeyObjectCollection<T> collection, KeyObjectEntry<T> entry)
        {
            KeyCollectionDrawerProperties.Remove(entry.Key.Id);
            
            collection.RemoveEntry(entry);
            
            _modelKeys.SetDirty(true);
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
    }
}