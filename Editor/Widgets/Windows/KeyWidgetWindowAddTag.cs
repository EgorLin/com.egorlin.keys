using System;
using System.Collections.Generic;
using EgorLin.Collections.Unsafe;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
    public class KeyWidgetWindowAddTag : EditorWindow
    {
        private static bool _arePassedTagsLocked;
        private string _search = "";
        private Vector2 _scroll;
        private readonly List<string> _results = new();
        private Action<string> _onSelected;
        private FastList<string> _passedTags = new();
        private Action _onClose;

        public static void Open(FastList<string> passedTags, bool areTagsLocked, Action<string> onSelected, Action onClose = null)
        {
            _arePassedTagsLocked = areTagsLocked;
            var window = CreateInstance<KeyWidgetWindowAddTag>();
            
            window.titleContent = new GUIContent(KeyWidgetWindowAddStyles.WindowTitle);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 
                KeyWidgetWindowAddStyles.WindowWidth, KeyWidgetWindowAddStyles.WindowHeight);
            window._onSelected = onSelected;
            window._onClose = onClose;
            
            window._passedTags = passedTags;
            window._passedTags ??= new FastList<string>();
            
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
            _search = _search.ToLowerInvariant();
            
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
            var resultsLabel = KeyWidgetWindowAddStyles.GetResultsLabel(_arePassedTagsLocked, _results.Count);
            EditorGUILayout.LabelField(resultsLabel, EditorStyles.boldLabel);
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, 
                KeyWidgetWindowAddStyles.LayoutOptionScrollMaxHeight);
            
            foreach (var tag in _results)
            {
                if (DrawTagButton(tag))
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
     
        private bool DrawTagButton(string tag)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            
            var icon = KeyWidgetWindowAddStyles.IconTag;
            
            if (KeyWidgetWindowAddStyles.DrawTagButton(icon, tag, _arePassedTagsLocked, KeyWidgetWindowAddStyles.GetTagButtonStyle()))
            {
                SelectTag(tag);
                EditorGUILayout.EndHorizontal();
                
                return true;
            }
            
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();
            
            return false;
        }

        private static bool HasInPassedTags(string checkValue, FastList<string> lockedValues)
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
            if (HasInPassedTags(_search, _passedTags))
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
            
            foreach (var keyTag in _passedTags)
            {
                _results.Add(keyTag);
            }
        }
    }
}