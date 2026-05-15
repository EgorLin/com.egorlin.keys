using System.Collections.Generic;
using EgorLin.Keys.Backend;
using EgorLin.Keys.Base.Commands;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.CodeGeneration;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Dialogs;
using EgorLin.Keys.Editor.Widgets.Items;
using EgorLin.Keys.Editor.Widgets.Paths;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Tags.Data;
using EgorLin.Pools;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    public class KeyCollectionDrawer : OdinValueDrawer<KeyCollection>
    {
        private readonly ModelKeyWidgetPathRoot _modelPath = new();
        private readonly ModelKeyItems<KeyTag> _modelKeys = new((item) => item);
        
        private bool _isSaveDirty;
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var collection = ValueEntry.SmartValue;
            
            KeyWidgetInfoBox.Draw();

            if (KeyWidgetSaveButton.DrawSaveButton(_isSaveDirty))
            {
                CommandKeyCollectionSaveAsset.Execute(Property);
                EditorApplication.delayCall += () =>
                {
                    _isSaveDirty = false;
                    
                    AssetDatabase.Refresh();
                    KeysBackend.Rebuild();
                };
            }

            if (KeyWidgetGenerateButton.DrawButton())
            {
                KeyCollectionCodeGenerator.Generate();
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
                SetDirty();
            }

            _modelKeys.SetTextSearch(textSearch);
        }

        private void DrawList(List<KeyTag> keys)
        {
            if (keys.Count == 0)
            {
                var hasSourceItems = keys.Count != 0;
                
                KeyWidgetItemList.DrawEmptyHelpBox(hasSourceItems, _modelKeys.Text);
            }
            else
            {
                var result = KeyWidgetItemList.DrawList(_modelKeys, (keyItem) => KeyWidgetItemRaw.Draw(keyItem, keyItem.Id));
                
                if (result.HasItemToRemove)
                {
                    RemoveItem(keys, _modelKeys.FilteredItems[result.Index]);
                }

                if (result.HasItemToRename)
                {
                    Rename(keys, _modelKeys.FilteredItems[result.Index]);
                }
            }
        }

        private void Clear(List<KeyTag> keys)
        {
            if (!KeyWidgetDialogClear.Draw(keys.Count))
            {
                return;
            }

            keys.Clear();
            
            SetDirty();
        }

        private void RemoveItem(List<KeyTag> keys, KeyTag key)
        {
            keys.Remove(key);
            
            SetDirty();
        }

        private void Rename(List<KeyTag> keys, KeyTag key)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
            
            foreach (var value in keys)
            {
                lockedTagIds.Add(value.Value);
            }
            
            KeyWidgetWindowAddTag.Open(lockedTagIds, tag =>
            {
                Rename(keys, key, tag);
                
                SetDirty();

                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }

        private static void Rename(List<KeyTag> keys, KeyTag key, string tag)
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

        private void OpenAdd(List<KeyTag> keys)
        {
            var lockedTagIds = PoolFastList<string>.Spawn();
            
            foreach (var value in keys)
            {
                lockedTagIds.Add(value.Value);
            }
            
            KeyWidgetWindowAddTag.Open(lockedTagIds, tagId =>
            {
                AddKey(keys, tagId);
                
                PoolFastList<string>.Recycle(lockedTagIds);
            });
        }

        private void AddKey(List<KeyTag> keys, string tag)
        {
            var keyItem = KeyTag.Create(tag);
            
            keys.Add(keyItem);
            
            SetDirty();
        }

        private void SetDirty()
        {
            _modelKeys.SetDirty(true);
            _isSaveDirty = true;
        }
    }
}