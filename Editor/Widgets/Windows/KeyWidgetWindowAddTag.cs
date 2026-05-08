using System;
using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using EgorLin.Keys.Backend.Database;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Commands;
using EgorLin.Keys.Tags.Data;
using EgorLin.Keys.Utils;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
    public class KeyWidgetWindowAddTag : EditorWindow
    {
        private string _search = "";
        private Vector2 _scroll;
        private readonly List<KeyTag> _results = new();
        private Action<KeyId> _onSelected;
        private FastList<KeyId> _lockedValues = new();
        private Action _onClose;

        public static void Open(FastList<KeyId> lockedTags, Action<KeyId> onSelected, Action onClose = null)
        {
            var window = CreateInstance<KeyWidgetWindowAddTag>();
            
            window.titleContent = new GUIContent(KeyWidgetWindowAddStyles.WindowTitle);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 
                KeyWidgetWindowAddStyles.WindowWidth, KeyWidgetWindowAddStyles.WindowHeight);
            window._onSelected = onSelected;
            window._onClose = onClose;
            
            window._lockedValues = lockedTags;
            window._lockedValues ??= new FastList<KeyId>();
            
            window.RefreshResults();
            window.ShowUtility();
        }

        private void OnDestroy()
        {
            _onClose?.Invoke();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            DrawHeader();
            DrawSearch();
            
            EditorGUILayout.Space(KeyWidgetWindowAddStyles.SpaceAfterSearch);
            
            if (!string.IsNullOrEmpty(_search))
            {
                DrawCreateNew();
                EditorGUILayout.Space(KeyWidgetWindowAddStyles.SpaceAfterCreateNew);
            }
            
            DrawResults();
            
            EditorGUILayout.EndVertical();
        }
     
        private void DrawHeader()
        {
            EditorGUILayout.LabelField(KeyWidgetWindowAddStyles.LabelHeader, 
                KeyWidgetWindowAddStyles.GetHeaderStyle());
        }
     
        private void DrawSearch()
        {
            EditorGUI.BeginChangeCheck();
            
            GUI.SetNextControlName(KeyWidgetWindowAddStyles.ControlNameSearchField);
            
            var searchFieldStyle = GUI.skin.FindStyle(KeyWidgetWindowAddStyles.StyleNameToolbarSearch) 
                ?? EditorStyles.textField;
            _search = EditorGUILayout.TextField(_search, searchFieldStyle);
            
            if (EditorGUI.EndChangeCheck())
            {
                RefreshResults();
            }
            
            if (Event.current.type == EventType.Layout)
            {
                EditorGUI.FocusTextInControl(KeyWidgetWindowAddStyles.ControlNameSearchField);
            }
        }
     
        private void DrawResults()
        {
            var resultsLabel = KeyWidgetWindowAddStyles.GetResultsLabel(_search, _results.Count);
            EditorGUILayout.LabelField(resultsLabel, EditorStyles.boldLabel);
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, 
                KeyWidgetWindowAddStyles.LayoutOptionScrollMaxHeight);
            
            foreach (var tag in _results)
            {
                if (DrawTagButton(tag, false, _lockedValues))
                {
                    EditorGUILayout.EndScrollView();
                    return;
                }
            }
            
            if (_results.Count == 0 && !string.IsNullOrEmpty(_search))
            {
                var message = string.Format(KeyWidgetWindowAddStyles.MessageNoResults, _search);
                EditorGUILayout.HelpBox(message, MessageType.Info);
            }
            
            EditorGUILayout.EndScrollView();
        }
     
        private bool DrawTagButton(KeyTag tag, bool isRecent, FastList<KeyId> lockedValues)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            var isLocked = HasInLocked(tag, lockedValues);
            
            var icon = isRecent ? KeyWidgetWindowAddStyles.IconRecent : KeyWidgetWindowAddStyles.IconTag;
            
            if (KeyWidgetWindowAddStyles.DrawTagButton(icon, tag.Value, isLocked, KeyWidgetWindowAddStyles.GetTagButtonStyle()))
            {
                SelectTag(tag.Id);
                EditorGUILayout.EndHorizontal();
                
                return true;
            }
            
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();
            
            return false;
        }

        private static bool HasInLocked(KeyTag tag, FastList<KeyId> lockedValues)
        {
            foreach (var value in lockedValues)
            {
                if (tag.Id == value)
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawCreateNew()
        {
            var hasTag = CommandKeyTagHas.Has(_search);
            
            if (hasTag)
            {
                var keyTag = CommandKeyTagGetTag.Execute(_search);

                if (HasInLocked(keyTag, _lockedValues))
                {
                    DrawLockedExistingButton(keyTag);
                }
                else
                {
                    DrawUseExistingButton(keyTag);
                }
            }
            else
            {
                DrawCreateNewButton();
            }
        }
        
        private void DrawLockedExistingButton(KeyTag existingTag)
        {
            var label = string.Format(KeyWidgetWindowAddStyles.LabelLockedTag, existingTag.Value);

            KeyWidgetWindowAddStyles.DrawColoredButton(label, KeyWidgetWindowAddStyles.ColorLockedTagButton,
                KeyWidgetWindowAddStyles.LayoutOptionCreateButtonHeight);
        }
        
        private void DrawUseExistingButton(KeyTag existingTag)
        {
            var label = string.Format(KeyWidgetWindowAddStyles.LabelUseExistingTag, existingTag.Value);
            
            if (KeyWidgetWindowAddStyles.DrawColoredButton(label, 
                KeyWidgetWindowAddStyles.ColorExistingTagButton, 
                KeyWidgetWindowAddStyles.LayoutOptionCreateButtonHeight))
            {
                SelectTag(existingTag.Id);
            }
        }
        
        private void DrawCreateNewButton()
        {
            var label = string.Format(KeyWidgetWindowAddStyles.LabelCreateNewTag, _search);
            
            if (KeyWidgetWindowAddStyles.DrawColoredButton(label, 
                KeyWidgetWindowAddStyles.ColorCreateNewButton, 
                KeyWidgetWindowAddStyles.LayoutOptionCreateButtonHeight))
            {
                var newId = CommandKeyTagGetOrCreateTagId.Execute(_search);
                SelectTag(newId);
            }
        }
     
        private void SelectTag(KeyId tagId)
        {
            _onSelected?.Invoke(tagId);
            Close();
        }
     
        private void RefreshResults()
        {
            _results.Clear();
            
            var database = KeyTagDatabaseProvider.Get();
            var tags = database.GetTags();

            var isSearchEmpty = string.IsNullOrEmpty(_search);
            var lowValue = _search.ToLowerInvariant();
            
            foreach (var keyTag in tags)
            {
                if (!keyTag.IsEmpty() && (isSearchEmpty || SearchUtils.FuzzyMatch(keyTag.Value, lowValue)))
                {
                    _results.Add(keyTag);
                }
            }
        }
    }
}