using System.Collections.Generic;
using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Backend.Indexers.Items;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using EgorLin.Keys.Selectors.Assets;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Selectors
{
    public class KeySelectorDrawer : OdinValueDrawer<KeySelector>
    {
        private GUIStyle pathStyle;
        private GUIStyle iconStyle;
        
        protected override void Initialize()
        {
            base.Initialize();
            InitializeStyles();
        }
        
        private void InitializeStyles()
        {
            pathStyle = KeySelectorDrawerStyles.CreatePathStyle();
            iconStyle = KeySelectorDrawerStyles.CreateIconStyle();
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var selector = ValueEntry.SmartValue;
            var keyId = selector.KeyId;
            
            DrawCollectionSourceControls(selector);
            
            DrawCompactSelector(selector, keyId);
        }
        
        private void DrawCollectionSourceControls(KeySelector selector)
        {
            EditorGUI.BeginChangeCheck();

            var toggle = EditorGUILayout.Toggle("Specific Collection", selector.isSpecificCollection);
            
            var asset = selector.specificCollection as Object;
            
            if (toggle)
            {
                asset = EditorGUILayout.ObjectField(
                    "Collection",
                    asset,
                    typeof(IKeyCollectionContainer),
                   true 
                );
            }

            if (EditorGUI.EndChangeCheck())
            {
                selector.isSpecificCollection = toggle;
                selector.specificCollection = asset;
                ValueEntry.SmartValue = selector;
            }
        }

        private void DrawCompactSelector(KeySelector selector, KeyId keyId)
        {
            var rect = EditorGUILayout.GetControlRect(false, KeySelectorDrawerStyles.CompactHeight);
            
            var bgColor = keyId.IsEmpty 
                ? KeySelectorDrawerStyles.ColorBackgroundEmpty
                : KeySelectorDrawerStyles.ColorBackgroundSet;
            
            EditorGUI.DrawRect(rect, bgColor);
            
            var iconRect = new Rect(
                rect.x + KeySelectorDrawerStyles.IconPadding, 
                rect.y + (rect.height - KeySelectorDrawerStyles.IconSize) / 2, 
                KeySelectorDrawerStyles.IconSize, 
                KeySelectorDrawerStyles.IconSize
            );
            
            var textRect = new Rect(
                iconRect.xMax,
                rect.y,
                rect.width - iconRect.width - KeySelectorDrawerStyles.IconPadding,
                rect.height
            );
            
            DrawIcon(iconRect, keyId.IsEmpty);
            DrawPathWithKey(textRect, keyId);
            
            HandleInput(rect, selector, keyId);
        }
        
        private void DrawIcon(Rect iconRect, bool isEmpty)
        {
            var icon = isEmpty ? KeySelectorDrawerStyles.IconEmpty : KeySelectorDrawerStyles.IconSet;
            GUI.Label(iconRect, icon, iconStyle);
        }
        
        private void DrawPathWithKey(Rect rect, KeyId keyId)
        {
            if (keyId.IsEmpty)
            {
                KeySelectorDrawerStyles.ConfigureButtonStyleForEmpty(pathStyle);
                GUI.Label(rect, KeySelectorDrawerStyles.LabelNoKeySelected, pathStyle);
                return;
            }
            
            KeySelectorDrawerStyles.ConfigureButtonStyleForSet(pathStyle);
            var fullPath = GetFullPathWithKey(keyId);
            GUI.Label(rect, fullPath, pathStyle);
        }
        
        private string GetFullPathWithKey(KeyId keyId)
        {
            var collection = KeyCollectionOwnerIndexer.Get(keyId);

            if (collection == null)
            {
                return $"INVALID {keyId.Hash} id ";
            }
            
            var parts = new List<string>();
            
            foreach (var pathNode in collection.GetAllPaths())
            {
                parts.Add(pathNode.Value);
            }
            
            var keyValue2 = KeyItemIndexer.GetValue(keyId);
            parts.Add(keyValue2.Value);
            
            return string.Join(" " + KeySelectorDrawerStyles.LabelPathArrow + " ", parts);
        }
        
        private void HandleInput(Rect rect, KeySelector selector, KeyId keyId)
        {
            var e = Event.current;
            
            if (!rect.Contains(e.mousePosition)) return;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                var container = selector.isSpecificCollection ? selector.specificCollection as IKeyCollectionContainer : null;
                KeyWidgetWindowSelectorSearch.Open(selector.SetKey, container);
                
                e.Use();
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                ShowContextMenu(selector, keyId);
                e.Use();
            }
        }
        
        private void ShowContextMenu(KeySelector selector, KeyId keyId)
        {
            var menu = new GenericMenu();
            
            if (!keyId.IsEmpty)
            {
                
                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemCopyHash), false, () =>
                {
                    EditorGUIUtility.systemCopyBuffer = keyId.Hash.ToString();
                    Debug.Log(string.Format(KeySelectorDrawerStyles.LogCopiedHash, keyId.Hash));
                });
                
                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemCopyPath), false, () =>
                {
                    var path = GetFullPathWithKey(keyId);
                    EditorGUIUtility.systemCopyBuffer = path;
                    Debug.Log(string.Format(KeySelectorDrawerStyles.LogCopiedPath, path));
                });
                
                menu.AddSeparator("");
                
                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemShowInProject), false, () =>
                {
                    var collectionOwner = KeyCollectionOwnerIndexer.Get(keyId);
                    var asset = collectionOwner.GetOwner();
                    
                    if (asset != null)
                    {
                        EditorGUIUtility.PingObject(asset);
                        Selection.activeObject = asset;
                    }
                });
                
                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemInfo), false, () =>
                {
                    ShowKeyInfo(keyId);
                });
                
                menu.AddSeparator("");
                
                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemClear), false, () =>
                {
                    selector.SetKey(KeyId.Empty);
                    GUI.changed = true;
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(KeySelectorDrawerStyles.MenuItemNoKeySelected));
            }
            
            menu.ShowAsContext();
        }
        
        private void ShowKeyInfo(KeyId keyId)
        {
            var keyValue = KeyItemIndexer.GetValue(keyId);
            
            var message = string.Format(KeySelectorDrawerStyles.DialogMessageKeyInfo,
                keyValue.Value,
                keyId.Hash,
                GetFullPathWithKey(keyId)
            );
            
            EditorUtility.DisplayDialog(
                KeySelectorDrawerStyles.DialogTitleKeyInfo,
                message,
                KeySelectorDrawerStyles.DialogButtonOK
            );
        }
    }
}