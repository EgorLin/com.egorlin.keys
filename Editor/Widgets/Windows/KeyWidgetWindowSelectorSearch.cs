using System;
using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Backend.Indexers.Collection;
using EgorLin.Keys.Base.Models;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Tags.Commands;
using EgorLin.Keys.Utils;
using EgorLin.Pools;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
    public class KeyWidgetWindowSelectorSearch : EditorWindow
    {
        private string _search = "";
        private Vector2 _scroll;
        private readonly List<ModelKeyCollectionEntrySearch> _results = new();
        private Action<KeyId> _onSelected;
        private readonly Dictionary<KeyId, int> _keyUsageCache = new();
     
        public static void Open(Action<KeyId> onSelected)
        {
            var window = CreateInstance<KeyWidgetWindowSelectorSearch>();
            window.titleContent = new GUIContent(KeyWidgetWindowSelectorSearchStyles.WindowTitle);
            window.position = new Rect(
                Screen.width / 2 - KeyWidgetWindowSelectorSearchStyles.WindowOffsetX, 
                Screen.height / 2 - KeyWidgetWindowSelectorSearchStyles.WindowOffsetY, 
                KeyWidgetWindowSelectorSearchStyles.WindowWidth, 
                KeyWidgetWindowSelectorSearchStyles.WindowHeight
            );
            window._onSelected = onSelected;
            window.ShowUtility();
            
            var resultsBuffer = PoolFastList<ModelKeyCollectionEntrySearch>.Spawn();

            FillCollections(resultsBuffer);
            
            CopyResults(window._results, resultsBuffer);
            PoolFastList<ModelKeyCollectionEntrySearch>.Recycle(resultsBuffer);
        }
     
        private void OnGUI()
        {
            DrawHeader();
            DrawSearch();
            
            EditorGUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceAfterSearchBar);
            
            DrawResults();
        }
     
        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField(
                KeyWidgetWindowSelectorSearchStyles.WindowTitle, 
                KeyWidgetWindowSelectorSearchStyles.CreateTitleStyle(), 
                GUILayout.Height(KeyWidgetWindowSelectorSearchStyles.TitleHeight)
            );
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceAfterHeader);
        }
     
        private void DrawSearch()
        {
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label(
                KeyWidgetWindowSelectorSearchStyles.IconSearch, 
                GUILayout.Width(KeyWidgetWindowSelectorSearchStyles.SearchIconWidth)
            );
            
            DrawSearchField();
            DrawClearButton();
            
            EditorGUILayout.EndHorizontal();
            
            FocusSearchField();
            
            EditorGUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceAfterSearch);
        }
        
        private void DrawSearchField()
        {
            EditorGUI.BeginChangeCheck();
            
            GUI.SetNextControlName(KeyWidgetWindowSelectorSearchStyles.ControlNameSearchField);
            
            var searchStyle = GUI.skin.FindStyle(KeyWidgetWindowSelectorSearchStyles.StyleNameToolbarSearch) 
                ?? EditorStyles.textField;
            _search = EditorGUILayout.TextField(_search, searchStyle);
            
            if (EditorGUI.EndChangeCheck())
            {
                FilterResults();
            }
        }
        
        private void DrawClearButton()
        {
            if (GUILayout.Button(
                KeyWidgetWindowSelectorSearchStyles.LabelSearchClear, 
                GUILayout.Width(KeyWidgetWindowSelectorSearchStyles.SearchClearButtonWidth)))
            {
                _search = "";
                FilterResults();
                GUI.FocusControl(KeyWidgetWindowSelectorSearchStyles.ControlNameSearchField);
            }
        }
        
        private void FocusSearchField()
        {
            if (Event.current.type == EventType.Layout)
            {
                EditorGUI.FocusTextInControl(KeyWidgetWindowSelectorSearchStyles.ControlNameSearchField);
            }
        }
     
        private void DrawResults()
        {
            var label = KeyWidgetWindowSelectorSearchStyles.GetResultsLabel(_search, _results.Count);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            
            if (_results.Count == 0)
            {
                DrawNoResultsMessage();
            }
            else
            {
                DrawResultsList();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawNoResultsMessage()
        {
            var message = KeyWidgetWindowSelectorSearchStyles.GetNoResultsMessage(_search);
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }
        
        private void DrawResultsList()
        {
            if (string.IsNullOrEmpty(_search))
            {
                DrawGroupedResults();
            }
            else
            {
                DrawFlatResults();
            }
        }
     
        private void DrawGroupedResults()
        {
            foreach (var collection in _results)
            {
                var path = collection.GetFullPath();
                DrawGroupHeader(path);
                DrawGroupEntries(collection, path);
                
                EditorGUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceBetweenGroups);
            }
        }
        
        private void DrawGroupHeader(string path)
        {
            var label = string.Format(KeyWidgetWindowSelectorSearchStyles.LabelGroupHeaderFormat, path);
            EditorGUILayout.LabelField(label, KeyWidgetWindowSelectorSearchStyles.CreateGroupHeaderStyle());
        }
        
        private void DrawGroupEntries(ModelKeyCollectionEntrySearch entry, string path)
        {
            GUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceBetweenEntries);
            
            foreach (var entryKey in entry.Keys)
            {
                DrawKeyEntry(entryKey, false, path);
            }
        }
     
        private void DrawFlatResults()
        {
            foreach (var entry in _results)
            {
                var path = entry.GetFullPath();
                foreach (var entryKey in entry.Keys)
                {
                    DrawKeyEntry(entryKey, false, path);
                    GUILayout.Space(KeyWidgetWindowSelectorSearchStyles.SpaceBetweenEntries);
                }
            }
        }
     
        private void DrawKeyEntry(KeyItem keyItem, bool isRecent, string path)
        {
            var rect = EditorGUILayout.GetControlRect(false, KeyWidgetWindowSelectorSearchStyles.EntryHeight);
            
            var isHovered = rect.Contains(Event.current.mousePosition);
            var bgColor = KeyWidgetWindowSelectorSearchStyles.GetBackgroundColor(isRecent, isHovered);
            
            EditorGUI.DrawRect(rect, bgColor);

            var keyValueTagId = keyItem.TagId;
            var keyTag = CommandKeyTagGetTag.Execute(keyValueTagId);

            DrawEntryLeftBorder(rect);
            DrawEntryIcon(rect, isRecent);
            DrawEntryName(rect, keyTag.Value);
            DrawEntryPath(rect, path);
            DrawEntryHash(rect, keyItem.Id);
            
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                SelectKey(keyItem);
            }
        }
        
        private void DrawEntryLeftBorder(Rect rect)
        {
            var color = Color.gray;
            
            var borderRect = new Rect(
                rect.x, 
                rect.y, 
                KeyWidgetWindowSelectorSearchStyles.EntryLeftBorderWidth, 
                rect.height
            );
            EditorGUI.DrawRect(borderRect, color);
        }
        
        private void DrawEntryIcon(Rect rect, bool isRecent)
        {
            var iconRect = new Rect(
                rect.x + KeyWidgetWindowSelectorSearchStyles.EntryIconPadding, 
                rect.y + (rect.height - KeyWidgetWindowSelectorSearchStyles.EntryIconSize) / 2, 
                KeyWidgetWindowSelectorSearchStyles.EntryIconSize, 
                KeyWidgetWindowSelectorSearchStyles.EntryIconSize
            );
            
            var icon = isRecent 
                ? KeyWidgetWindowSelectorSearchStyles.IconRecent 
                : KeyWidgetWindowSelectorSearchStyles.IconKey;
            
            GUI.Label(iconRect, icon, KeyWidgetWindowSelectorSearchStyles.CreateIconStyle());
        }
        
        private void DrawEntryName(Rect rect, string tagValue)
        {
            var iconWidth = KeyWidgetWindowSelectorSearchStyles.EntryIconPadding + KeyWidgetWindowSelectorSearchStyles.EntryIconSize;
            var nameRect = new Rect(
                rect.x + iconWidth + KeyWidgetWindowSelectorSearchStyles.IconTextGap, 
                rect.y + 8, 
                rect.width - KeyWidgetWindowSelectorSearchStyles.EntryContentRightReserve, 
                20
            );
            
            GUI.Label(nameRect, tagValue, KeyWidgetWindowSelectorSearchStyles.CreateEntryNameStyle());
        }
        
        private void DrawEntryPath(Rect rect, string path)
        {
            var iconWidth = KeyWidgetWindowSelectorSearchStyles.EntryIconPadding + KeyWidgetWindowSelectorSearchStyles.EntryIconSize;
            var pathRect = new Rect(
                rect.x + iconWidth + KeyWidgetWindowSelectorSearchStyles.IconTextGap, 
                rect.y + 8 + 20 + KeyWidgetWindowSelectorSearchStyles.EntryPathOffsetY, 
                rect.width - KeyWidgetWindowSelectorSearchStyles.EntryContentRightReserve, 
                18
            );
            
            GUI.Label(pathRect, path, KeyWidgetWindowSelectorSearchStyles.CreatePathStyle());
        }
        
        private void DrawEntryHash(Rect rect, KeyId keyId)
        {
            var hashRect = new Rect(
                rect.xMax - KeyWidgetWindowSelectorSearchStyles.EntryHashOffset, 
                rect.yMax - 18, 
                KeyWidgetWindowSelectorSearchStyles.EntryHashWidth, 
                16
            );
            
            var hashLabel = string.Format(KeyWidgetWindowSelectorSearchStyles.LabelHashFormat, keyId.ToString());
            GUI.Label(hashRect, hashLabel, KeyWidgetWindowSelectorSearchStyles.CreateHashStyle());
        }
     
        private void SelectKey(KeyItem item)
        {
            _onSelected?.Invoke(item.Id);
            Close();
        }
     
        private static void FillCollections(FastList<ModelKeyCollectionEntrySearch> buffer)
        {
            var collectionOwners = KeyCollectionOwnerIndexer.GetAllAssets();

            foreach (var collection in collectionOwners)
            {
                buffer.Add(new ModelKeyCollectionEntrySearch
                {
                    Paths = new List<KeyItem>(collection.GetAllPaths()),
                    Keys = new List<KeyItem>(collection.GetAllKeys()),
                });
            }
        }
     
        private void FilterResults()
        {
            _results.Clear();
            _keyUsageCache.Clear();

            var resultsBuffer = PoolFastList<ModelKeyCollectionEntrySearch>.Spawn();

            FillCollections(resultsBuffer);
            
            if (string.IsNullOrEmpty(_search))
            {
                CopyResults(_results, resultsBuffer);
                
                PoolFastList<ModelKeyCollectionEntrySearch>.Recycle(resultsBuffer);
                
                return;
            }
            
            var searchFormated = KeyTagUtils.Format(_search);
            
            foreach (var check in resultsBuffer)
            {
                if (TryMatch(check, searchFormated, out var result))
                {
                    _results.Add(result);
                }
            }
            
            PoolFastList<ModelKeyCollectionEntrySearch>.Recycle(resultsBuffer);
        }

        private static bool TryMatch(ModelKeyCollectionEntrySearch entryToCheck, string searchFormated,
            out ModelKeyCollectionEntrySearch result)
        {
            foreach (var resultPathPart in entryToCheck.Paths)
            {
                var keyTag = CommandKeyTagGetTag.Execute(resultPathPart.TagId);

                var matched = SearchUtils.FuzzyMatch(keyTag.Value, searchFormated);

                if (matched)
                {
                    result = new ModelKeyCollectionEntrySearch()
                    {
                        Keys = entryToCheck.Keys,
                        Paths = entryToCheck.Paths
                    };
                    
                    return true;
                }
            }

            var resultKeys = new List<KeyItem>();

            foreach (var keyValue in entryToCheck.Keys)
            {
                var tag = CommandKeyTagGetTag.Execute(keyValue.TagId);
                var matchedTag = SearchUtils.FuzzyMatch(tag.Value, searchFormated);

                if (matchedTag)
                {
                    resultKeys.Add(keyValue);
                }
            }

            if (resultKeys.Count > 0)
            {
                result = new ModelKeyCollectionEntrySearch()
                {
                    Paths = entryToCheck.Paths,
                    Keys = resultKeys,
                };
                
                return true;
            }

            result = default;
            return false;
        }

        private static void CopyResults(List<ModelKeyCollectionEntrySearch> destination, PooledFastList<ModelKeyCollectionEntrySearch> resultsBuffer)
        {
            foreach (var entry in resultsBuffer)
            {
                destination.Add(entry);
            }
        }
    }
}
