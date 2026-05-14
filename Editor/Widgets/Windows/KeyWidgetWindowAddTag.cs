using System;
using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
    public class KeyWidgetWindowAddTag : EditorWindow
    {
        private string _search = "";
        private Vector2 _scroll;
        private readonly List<string> _results = new();
        private Action<string> _onSelected;
        private FastList<string> _lockedValues = new();
        private Action _onClose;

        public static void Open(FastList<string> lockedTags, Action<string> onSelected, Action onClose = null)
        {
            var window = CreateInstance<KeyWidgetWindowAddTag>();
            
            window.titleContent = new GUIContent(KeyWidgetWindowAddStyles.WindowTitle);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 
                KeyWidgetWindowAddStyles.WindowWidth, KeyWidgetWindowAddStyles.WindowHeight);
            window._onSelected = onSelected;
            window._onClose = onClose;
            
            window._lockedValues = lockedTags;
            window._lockedValues ??= new FastList<string>();
            
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
     
        private bool DrawTagButton(string tag, bool isRecent, FastList<string> lockedValues)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            var isLocked = HasInLocked(tag, lockedValues);
            
            var icon = isRecent ? KeyWidgetWindowAddStyles.IconRecent : KeyWidgetWindowAddStyles.IconTag;
            
            if (KeyWidgetWindowAddStyles.DrawTagButton(icon, tag, isLocked, KeyWidgetWindowAddStyles.GetTagButtonStyle()))
            {
                SelectTag(tag);
                EditorGUILayout.EndHorizontal();
                
                return true;
            }
            
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();
            
            return false;
        }

        private static bool HasInLocked(string checkValue, FastList<string> lockedValues)
        {
            foreach (var value in lockedValues)
            {
                if (checkValue == value)
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawCreateNew()
        {
            if (HasInLocked(_search, _lockedValues))
            {
                DrawLockedExistingButton(_search);
            }
            else
            {
                DrawCreateNewButton();
            }
        }
        
        private void DrawLockedExistingButton(string existingTag)
        {
            var label = string.Format(KeyWidgetWindowAddStyles.LabelLockedTag, existingTag);

            KeyWidgetWindowAddStyles.DrawColoredButton(label, KeyWidgetWindowAddStyles.ColorLockedTagButton,
                KeyWidgetWindowAddStyles.LayoutOptionCreateButtonHeight);
        }
        
        private void DrawCreateNewButton()
        {
            var label = string.Format(KeyWidgetWindowAddStyles.LabelCreateNewTag, _search);
            
            if (KeyWidgetWindowAddStyles.DrawColoredButton(label, 
                KeyWidgetWindowAddStyles.ColorCreateNewButton, 
                KeyWidgetWindowAddStyles.LayoutOptionCreateButtonHeight))
            {
                SelectTag(_search);
            }
        }
     
        private void SelectTag(string tag)
        {
            _onSelected?.Invoke(tag);
            Close();
        }
     
        private void RefreshResults()
        {
            _results.Clear();
            
            foreach (var keyTag in _lockedValues)
            {
                _results.Add(keyTag);
            }
        }
    }
}