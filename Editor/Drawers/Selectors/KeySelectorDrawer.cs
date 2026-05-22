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
        private GUIStyle _headerLabelStyle;
        private GUIStyle _badgeStyle;
        private GUIStyle _sourceLabelStyle;
        private GUIStyle _keyPathStyle;
        private GUIStyle _keyPathEmptyStyle;
        private GUIStyle _iconStyle;
        private GUIStyle _collectionNameStyle;
        private GUIStyle _collectionNameAssignedStyle;

        // Track hover state for the clear button
        private bool _clearHovered;

        protected override void Initialize()
        {
            base.Initialize();
            _headerLabelStyle             = KeySelectorDrawerStyles.CreateHeaderLabelStyle();
            _badgeStyle                   = KeySelectorDrawerStyles.CreateBadgeStyle();
            _sourceLabelStyle             = KeySelectorDrawerStyles.CreateSourceLabelStyle();
            _keyPathStyle                 = KeySelectorDrawerStyles.CreateKeyPathStyle();
            _keyPathEmptyStyle            = KeySelectorDrawerStyles.CreateKeyPathEmptyStyle();
            _iconStyle                    = KeySelectorDrawerStyles.CreateIconStyle();
            _collectionNameStyle          = KeySelectorDrawerStyles.CreateCollectionNameStyle();
            _collectionNameAssignedStyle  = KeySelectorDrawerStyles.CreateCollectionNameAssignedStyle();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var selector = ValueEntry.SmartValue;
            var keyId    = selector.KeyId;

            float totalHeight =
                KeySelectorDrawerStyles.GroupHeaderHeight +
                1f + // header border
                KeySelectorDrawerStyles.BodyPadding +
                KeySelectorDrawerStyles.SourceRowHeight +
                KeySelectorDrawerStyles.RowGap +
                KeySelectorDrawerStyles.KeyRowHeight +
                KeySelectorDrawerStyles.BodyPadding;

            var groupRect = EditorGUILayout.GetControlRect(false, totalHeight);

            DrawGroupBackground(groupRect);
            DrawAccentBorder(groupRect);
            DrawGroupBorder(groupRect);

            var headerRect = new Rect(
                groupRect.x, groupRect.y,
                groupRect.width, KeySelectorDrawerStyles.GroupHeaderHeight);

            DrawHeader(headerRect, label, keyId);

            float bodyY = headerRect.yMax + 1f;
            float bodyX = groupRect.x + KeySelectorDrawerStyles.AccentBorderWidth;
            float bodyW = groupRect.width - KeySelectorDrawerStyles.AccentBorderWidth;

            var sourceRect = new Rect(
                bodyX + KeySelectorDrawerStyles.BodyPadding,
                bodyY + KeySelectorDrawerStyles.BodyPadding,
                bodyW - KeySelectorDrawerStyles.BodyPadding * 2,
                KeySelectorDrawerStyles.SourceRowHeight);

            var keyRect = new Rect(
                sourceRect.x,
                sourceRect.yMax + KeySelectorDrawerStyles.RowGap,
                sourceRect.width,
                KeySelectorDrawerStyles.KeyRowHeight);

            bool changed = DrawSourceRow(sourceRect, selector);
            DrawKeyRow(keyRect, selector, keyId);

            if (changed)
                ValueEntry.SmartValue = selector;
        }

        // ─── Group chrome ────────────────────────────────────────────

        private void DrawGroupBackground(Rect rect)
        {
            // Body
            EditorGUI.DrawRect(rect, KeySelectorDrawerStyles.ColorBodyBackground);

            // Header overlay
            var headerRect = new Rect(rect.x, rect.y,
                rect.width, KeySelectorDrawerStyles.GroupHeaderHeight);
            EditorGUI.DrawRect(headerRect, KeySelectorDrawerStyles.ColorHeaderBackground);
        }

        private void DrawAccentBorder(Rect rect)
        {
            var accentRect = new Rect(
                rect.x, rect.y,
                KeySelectorDrawerStyles.AccentBorderWidth, rect.height);
            EditorGUI.DrawRect(accentRect, KeySelectorDrawerStyles.ColorAccentBorder);
        }

        private void DrawGroupBorder(Rect rect)
        {
            // 1px border simulation via outline rects (EditorGUI has no outline primitive)
            float t = 1f;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - t, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, t, rect.height),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.xMax - t, rect.y, t, rect.height),
                KeySelectorDrawerStyles.ColorGroupBorder);

            // Header bottom divider
            EditorGUI.DrawRect(
                new Rect(rect.x, rect.y + KeySelectorDrawerStyles.GroupHeaderHeight, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
        }

        // ─── Header ──────────────────────────────────────────────────

        private void DrawHeader(Rect rect, GUIContent label, KeyId keyId)
        {
            // Field name (uppercase via style)
            var labelRect = new Rect(
                rect.x + KeySelectorDrawerStyles.AccentBorderWidth + 4f,
                rect.y,
                rect.width * 0.65f,
                rect.height);

            var displayLabel = label != null && !string.IsNullOrEmpty(label.text)
                ? label.text.ToUpper()
                : "KEY SELECTOR";

            GUI.Label(labelRect, displayLabel, _headerLabelStyle);

            // Status badge
            bool isSet       = !keyId.IsEmpty;
            var  badgeText   = isSet
                ? KeySelectorDrawerStyles.LabelAssigned
                : KeySelectorDrawerStyles.LabelUnassigned;
            var  badgeBgColor = isSet
                ? KeySelectorDrawerStyles.ColorBadgeSetBg
                : KeySelectorDrawerStyles.ColorBadgeEmptyBg;
            var  badgeTextColor = isSet
                ? KeySelectorDrawerStyles.ColorBadgeSetText
                : KeySelectorDrawerStyles.ColorBadgeEmptyText;

            float badgeW  = 62f;
            float badgeH  = 14f;
            var   badgeRect = new Rect(
                rect.xMax - badgeW - 6f,
                rect.y + (rect.height - badgeH) / 2f,
                badgeW, badgeH);

            EditorGUI.DrawRect(badgeRect, badgeBgColor);
            _badgeStyle.normal.textColor = badgeTextColor;
            GUI.Label(badgeRect, badgeText, _badgeStyle);
        }

        // ─── Source row ───────────────────────────────────────────────

        private bool DrawSourceRow(Rect rect, KeySelector selector)
        {
            bool changed = false;

            // "From" label
            float fromW    = 32f;
            var   fromRect = new Rect(rect.x, rect.y, fromW, rect.height);
            GUI.Label(fromRect, KeySelectorDrawerStyles.LabelFrom, _sourceLabelStyle);

            // Toggle pill
            float toggleX    = fromRect.xMax + 4f;
            var   toggleRect = new Rect(
                toggleX,
                rect.y + (rect.height - KeySelectorDrawerStyles.ToggleHeight) / 2f,
                KeySelectorDrawerStyles.ToggleWidth,
                KeySelectorDrawerStyles.ToggleHeight);

            bool newToggle = DrawTogglePill(toggleRect, selector.isSpecificCollection);
            if (newToggle != selector.isSpecificCollection)
            {
                selector.isSpecificCollection = newToggle;
                changed = true;
            }

            // Collection object field
            float fieldX   = toggleRect.xMax + 6f;
            var   fieldRect = new Rect(fieldX, rect.y, rect.xMax - fieldX, rect.height);

            if (selector.isSpecificCollection)
            {
                EditorGUI.BeginChangeCheck();
                var newAsset = DrawCollectionField(fieldRect, selector.specificCollection as Object, false);
                if (EditorGUI.EndChangeCheck())
                {
                    selector.specificCollection = newAsset;
                    changed = true;
                }
            }
            else
            {
                DrawCollectionField(fieldRect, null, true);
            }

            return changed;
        }

        private bool DrawTogglePill(Rect rect, bool isOn)
        {
            EditorGUI.DrawRect(rect, isOn
                ? KeySelectorDrawerStyles.ColorToggleOn
                : KeySelectorDrawerStyles.ColorToggleOff);

            float thumbSize = rect.height - 4f;
            float thumbX    = isOn ? rect.xMax - thumbSize - 2f : rect.x + 2f;
            EditorGUI.DrawRect(
                new Rect(thumbX, rect.y + 2f, thumbSize, thumbSize),
                KeySelectorDrawerStyles.ColorToggleThumb);

            var e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && rect.Contains(e.mousePosition))
            {
                GUI.changed = true;
                e.Use();
                return !isOn;
            }

            return isOn;
        }

        private Object DrawCollectionField(Rect rect, Object current, bool disabled)
        {
            EditorGUI.DrawRect(rect, KeySelectorDrawerStyles.ColorRowBackground);
            DrawThinBorder(rect, disabled
                ? new Color(
                    KeySelectorDrawerStyles.ColorRowBorder.r,
                    KeySelectorDrawerStyles.ColorRowBorder.g,
                    KeySelectorDrawerStyles.ColorRowBorder.b,
                    KeySelectorDrawerStyles.ColorRowBorder.a * 0.4f)
                : KeySelectorDrawerStyles.ColorRowBorder);

            if (disabled)
            {
                GUI.Label(
                    new Rect(rect.x + 6f, rect.y, rect.width - 8f, rect.height),
                    KeySelectorDrawerStyles.LabelAnyCollection,
                    _collectionNameStyle);
                return null;
            }

            using var _ = new EditorGUI.DisabledScope(false);

            var prevSkin       = EditorStyles.objectField.normal.background;
            var prevActive     = EditorStyles.objectField.active.background;
            var prevFocused    = EditorStyles.objectField.focused.background;
            var prevTextColor  = EditorStyles.objectField.normal.textColor;

            EditorStyles.objectField.normal.background   = null;
            EditorStyles.objectField.active.background   = null;
            EditorStyles.objectField.focused.background  = null;
            EditorStyles.objectField.normal.textColor    = current != null
                ? KeySelectorDrawerStyles.ColorTextPrimary
                : KeySelectorDrawerStyles.ColorTextSecondary;
            EditorStyles.objectField.fontSize            = 11;
            EditorStyles.objectField.padding             = new RectOffset(6, 4, 0, 0);

            var result = EditorGUI.ObjectField(rect, current, typeof(Object), true);

            
            EditorStyles.objectField.normal.background   = prevSkin;
            EditorStyles.objectField.active.background   = prevActive;
            EditorStyles.objectField.focused.background  = prevFocused;
            EditorStyles.objectField.normal.textColor    = prevTextColor;

            return result;
        }

        // ─── Key row ──────────────────────────────────────────────────

        private void DrawKeyRow(Rect rect, KeySelector selector, KeyId keyId)
        {
            bool  isEmpty = keyId.IsEmpty;
            var   bgColor = KeySelectorDrawerStyles.ColorRowBackground;

            EditorGUI.DrawRect(rect, bgColor);
            DrawThinBorder(rect, KeySelectorDrawerStyles.ColorRowBorder);

            // Icon column
            var iconRect = new Rect(rect.x, rect.y,
                KeySelectorDrawerStyles.IconColumnWidth, rect.height);

            var iconBgColor = isEmpty
                ? KeySelectorDrawerStyles.ColorIconBgEmpty
                : KeySelectorDrawerStyles.ColorIconBgSet;

            EditorGUI.DrawRect(iconRect, iconBgColor);

            // Right border on icon column
            EditorGUI.DrawRect(
                new Rect(iconRect.xMax - 1f, iconRect.y, 1f, iconRect.height),
                KeySelectorDrawerStyles.ColorRowBorder);

            _iconStyle.normal.textColor = isEmpty
                ? KeySelectorDrawerStyles.ColorIconFgEmpty
                : KeySelectorDrawerStyles.ColorIconFgSet;

            GUI.Label(iconRect,
                isEmpty ? KeySelectorDrawerStyles.IconWarning : KeySelectorDrawerStyles.IconKey,
                _iconStyle);

            // Clear button (right side)
            var clearRect = new Rect(
                rect.xMax - KeySelectorDrawerStyles.ClearButtonWidth, rect.y,
                KeySelectorDrawerStyles.ClearButtonWidth, rect.height);

            // Path text
            var pathRect = new Rect(
                iconRect.xMax, rect.y,
                rect.width - iconRect.width - clearRect.width, rect.height);

            if (isEmpty)
            {
                GUI.Label(pathRect, KeySelectorDrawerStyles.LabelNoKeySelected, _keyPathEmptyStyle);
            }
            else
            {
                var path = GetFullPathWithKey(keyId);
                GUI.Label(pathRect, path, _keyPathStyle);
            }

            // Draw clear button
            if (!isEmpty)
            {
                var e = Event.current;
                bool hovered = clearRect.Contains(e.mousePosition);

                if (hovered)
                    EditorGUI.DrawRect(clearRect,
                        new Color(KeySelectorDrawerStyles.ColorIconBgEmpty.r,
                                   KeySelectorDrawerStyles.ColorIconBgEmpty.g,
                                   KeySelectorDrawerStyles.ColorIconBgEmpty.b, 0.35f));

                _iconStyle.normal.textColor = hovered
                    ? KeySelectorDrawerStyles.ColorClearHover
                    : KeySelectorDrawerStyles.ColorTextHint;

                GUI.Label(clearRect, "✕", _iconStyle);

                if (e.type == EventType.MouseDown && e.button == 0 && hovered)
                {
                    selector.SetKey(KeyId.Empty);
                    ValueEntry.SmartValue = selector;
                    GUI.changed = true;
                    e.Use();
                    return;
                }
            }

            // Left-click opens selector, right-click opens context menu
            HandleKeyRowInput(rect, clearRect, selector, keyId);
        }

        private void HandleKeyRowInput(Rect rect, Rect clearRect, KeySelector selector, KeyId keyId)
        {
            var e = Event.current;
            if (!rect.Contains(e.mousePosition)) return;

            if (e.type == EventType.MouseDown && e.button == 0 && !clearRect.Contains(e.mousePosition))
            {
                var container = selector.isSpecificCollection
                    ? selector.specificCollection as IKeyCollectionContainer
                    : null;
                KeyWidgetWindowSelectorSearch.Open(selector.SetKey, container);
                e.Use();
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                ShowContextMenu(selector, keyId);
                e.Use();
            }
        }

        // ─── Shared helpers ───────────────────────────────────────────

        private void DrawThinBorder(Rect rect, Color color)
        {
            const float t = 1f;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - t, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, t, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - t, rect.y, t, rect.height), color);
        }

        private string GetFullPathWithKey(KeyId keyId)
        {
            var collection = KeyCollectionOwnerIndexer.Get(keyId);
            if (collection == null)
                return $"INVALID ({keyId.Hash})";

            var parts = new List<string>();
            foreach (var pathNode in collection.GetAllPaths())
                parts.Add(pathNode.Value);

            var keyValue = KeyItemIndexer.GetValue(keyId);
            parts.Add(keyValue.Value);

            return string.Join(KeySelectorDrawerStyles.PathArrow, parts);
        }

        // ─── Context menu ─────────────────────────────────────────────

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
                    var owner = KeyCollectionOwnerIndexer.Get(keyId);
                    var asset = owner?.GetOwner();
                    if (asset != null)
                    {
                        EditorGUIUtility.PingObject(asset);
                        Selection.activeObject = asset;
                    }
                });

                menu.AddItem(new GUIContent(KeySelectorDrawerStyles.MenuItemInfo), false, () =>
                    ShowKeyInfo(keyId));

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
            var message  = string.Format(
                KeySelectorDrawerStyles.DialogMessageKeyInfo,
                keyValue.Value,
                keyId.Hash,
                GetFullPathWithKey(keyId));

            EditorUtility.DisplayDialog(
                KeySelectorDrawerStyles.DialogTitleKeyInfo,
                message,
                KeySelectorDrawerStyles.DialogButtonOK);
        }
    }
}