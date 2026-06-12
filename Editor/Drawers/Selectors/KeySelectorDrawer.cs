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
    [CustomPropertyDrawer(typeof(KeySelector))]
    public class KeySelectorDrawer : PropertyDrawer
    {
        private GUIStyle _headerLabelStyle;
        private GUIStyle _badgeStyle;
        private GUIStyle _sourceLabelStyle;
        private GUIStyle _keyPathStyle;
        private GUIStyle _keyPathEmptyStyle;
        private GUIStyle _iconStyle;
        private GUIStyle _collectionNameStyle;
        private GUIStyle _collectionNameAssignedStyle;

        private bool _stylesInitialized;

        private void EnsureStyles()
        {
            if (_stylesInitialized) return;
            _stylesInitialized           = true;
            _headerLabelStyle            = KeySelectorDrawerStyles.CreateHeaderLabelStyle();
            _badgeStyle                  = KeySelectorDrawerStyles.CreateBadgeStyle();
            _sourceLabelStyle            = KeySelectorDrawerStyles.CreateSourceLabelStyle();
            _keyPathStyle                = KeySelectorDrawerStyles.CreateKeyPathStyle();
            _keyPathEmptyStyle           = KeySelectorDrawerStyles.CreateKeyPathEmptyStyle();
            _iconStyle                   = KeySelectorDrawerStyles.CreateIconStyle();
            _collectionNameStyle         = KeySelectorDrawerStyles.CreateCollectionNameStyle();
            _collectionNameAssignedStyle = KeySelectorDrawerStyles.CreateCollectionNameAssignedStyle();
        }

        // ─── Height ───────────────────────────────────────────────────────────────

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return KeySelectorDrawerStyles.GroupHeaderHeight
                 + 1f  // header border
                 + KeySelectorDrawerStyles.BodyPadding
                 + KeySelectorDrawerStyles.SourceRowHeight
                 + KeySelectorDrawerStyles.RowGap
                 + KeySelectorDrawerStyles.KeyRowHeight
                 + KeySelectorDrawerStyles.BodyPadding;
        }

        // ─── Main draw ────────────────────────────────────────────────────────────

        public override void OnGUI(Rect groupRect, SerializedProperty property, GUIContent label)
        {
            EnsureStyles();

            // Resolve child properties
            var propIsSpecific  = property.FindPropertyRelative(nameof(KeySelector.isSpecificCollection));
            var propCollection  = property.FindPropertyRelative(nameof(KeySelector.specificCollection));
            // In OnGUI, replace the single propKeyHash line with:
            var propId = property.FindPropertyRelative("id");
            var propKeyHash   = propId.FindPropertyRelative(nameof(KeyId.Hash));

            var keyId = KeyId.Create(propKeyHash.intValue); // int, not long

            DrawGroupBackground(groupRect);
            DrawAccentBorder(groupRect);
            DrawGroupBorder(groupRect);

            // Header
            var headerRect = new Rect(
                groupRect.x, groupRect.y,
                groupRect.width, KeySelectorDrawerStyles.GroupHeaderHeight);

            DrawHeader(headerRect, label, keyId);

            // Body layout
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

            EditorGUI.BeginChangeCheck();
            DrawSourceRow(sourceRect, propIsSpecific, propCollection);
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            DrawKeyRow(keyRect, propIsSpecific, propCollection, propKeyHash, keyId);
        }

        private void DrawGroupBackground(Rect rect)
        {
            EditorGUI.DrawRect(rect, KeySelectorDrawerStyles.ColorBodyBackground);

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
            const float t = 1f;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - t, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, t, rect.height),
                KeySelectorDrawerStyles.ColorGroupBorder);
            EditorGUI.DrawRect(new Rect(rect.xMax - t, rect.y, t, rect.height),
                KeySelectorDrawerStyles.ColorGroupBorder);

            // Header divider
            EditorGUI.DrawRect(
                new Rect(rect.x, rect.y + KeySelectorDrawerStyles.GroupHeaderHeight, rect.width, t),
                KeySelectorDrawerStyles.ColorGroupBorder);
        }

        // ─── Header ───────────────────────────────────────────────────────────────

        private void DrawHeader(Rect rect, GUIContent label, KeyId keyId)
        {
            var labelRect = new Rect(
                rect.x + KeySelectorDrawerStyles.AccentBorderWidth + 4f,
                rect.y,
                rect.width * 0.65f,
                rect.height);

            var displayLabel = label != null && !string.IsNullOrEmpty(label.text)
                ? label.text.ToUpper()
                : "KEY SELECTOR";

            GUI.Label(labelRect, displayLabel, _headerLabelStyle);

            bool isSet        = !keyId.IsEmpty;
            var  badgeText    = isSet ? KeySelectorDrawerStyles.LabelAssigned : KeySelectorDrawerStyles.LabelUnassigned;
            var  badgeBgColor = isSet ? KeySelectorDrawerStyles.ColorBadgeSetBg : KeySelectorDrawerStyles.ColorBadgeEmptyBg;
            var  badgeTxColor = isSet ? KeySelectorDrawerStyles.ColorBadgeSetText : KeySelectorDrawerStyles.ColorBadgeEmptyText;

            const float badgeW = 62f, badgeH = 14f;
            var badgeRect = new Rect(
                rect.xMax - badgeW - 6f,
                rect.y + (rect.height - badgeH) / 2f,
                badgeW, badgeH);

            EditorGUI.DrawRect(badgeRect, badgeBgColor);
            _badgeStyle.normal.textColor = badgeTxColor;
            GUI.Label(badgeRect, badgeText, _badgeStyle);
        }

        // ─── Source row ───────────────────────────────────────────────────────────

        private void DrawSourceRow(Rect rect,
            SerializedProperty propIsSpecific,
            SerializedProperty propCollection)
        {
            // "From" label
            const float fromW = 32f;
            GUI.Label(new Rect(rect.x, rect.y, fromW, rect.height),
                KeySelectorDrawerStyles.LabelFrom, _sourceLabelStyle);

            // Toggle pill
            float toggleX    = rect.x + fromW + 4f;
            var   toggleRect = new Rect(
                toggleX,
                rect.y + (rect.height - KeySelectorDrawerStyles.ToggleHeight) / 2f,
                KeySelectorDrawerStyles.ToggleWidth,
                KeySelectorDrawerStyles.ToggleHeight);

            bool newToggle = DrawTogglePill(toggleRect, propIsSpecific.boolValue);
            if (newToggle != propIsSpecific.boolValue)
                propIsSpecific.boolValue = newToggle;

            // Collection object field
            float fieldX   = toggleRect.xMax + 6f;
            var   fieldRect = new Rect(fieldX, rect.y, rect.xMax - fieldX, rect.height);

            if (propIsSpecific.boolValue)
            {
                var newAsset = DrawCollectionField(fieldRect, propCollection.objectReferenceValue, false);
                if (newAsset != propCollection.objectReferenceValue)
                    propCollection.objectReferenceValue = newAsset;
            }
            else
            {
                DrawCollectionField(fieldRect, null, true);
            }
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

            // Temporarily strip the default ObjectField chrome
            var prevSkin      = EditorStyles.objectField.normal.background;
            var prevActive    = EditorStyles.objectField.active.background;
            var prevFocused   = EditorStyles.objectField.focused.background;
            var prevTextColor = EditorStyles.objectField.normal.textColor;

            EditorStyles.objectField.normal.background  = null;
            EditorStyles.objectField.active.background  = null;
            EditorStyles.objectField.focused.background = null;
            EditorStyles.objectField.normal.textColor   = current != null
                ? KeySelectorDrawerStyles.ColorTextPrimary
                : KeySelectorDrawerStyles.ColorTextSecondary;
            EditorStyles.objectField.fontSize  = 11;
            EditorStyles.objectField.padding   = new RectOffset(6, 4, 0, 0);

            var result = EditorGUI.ObjectField(rect, current, typeof(Object), true);

            EditorStyles.objectField.normal.background  = prevSkin;
            EditorStyles.objectField.active.background  = prevActive;
            EditorStyles.objectField.focused.background = prevFocused;
            EditorStyles.objectField.normal.textColor   = prevTextColor;

            return result;
        }

        // ─── Key row ──────────────────────────────────────────────────────────────

        private void DrawKeyRow(Rect rect,
            SerializedProperty propIsSpecific,
            SerializedProperty propCollection,
            SerializedProperty propKeyHash,
            KeyId keyId)
        {
            bool isEmpty = keyId.IsEmpty;

            EditorGUI.DrawRect(rect, KeySelectorDrawerStyles.ColorRowBackground);
            DrawThinBorder(rect, KeySelectorDrawerStyles.ColorRowBorder);

            // Icon column
            var iconRect    = new Rect(rect.x, rect.y, KeySelectorDrawerStyles.IconColumnWidth, rect.height);
            var iconBgColor = isEmpty
                ? KeySelectorDrawerStyles.ColorIconBgEmpty
                : KeySelectorDrawerStyles.ColorIconBgSet;

            EditorGUI.DrawRect(iconRect, iconBgColor);
            EditorGUI.DrawRect(
                new Rect(iconRect.xMax - 1f, iconRect.y, 1f, iconRect.height),
                KeySelectorDrawerStyles.ColorRowBorder);

            _iconStyle.normal.textColor = isEmpty
                ? KeySelectorDrawerStyles.ColorIconFgEmpty
                : KeySelectorDrawerStyles.ColorIconFgSet;

            GUI.Label(iconRect,
                isEmpty ? KeySelectorDrawerStyles.IconWarning : KeySelectorDrawerStyles.IconKey,
                _iconStyle);

            // Clear button
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
                GUI.Label(pathRect, GetFullPathWithKey(keyId), _keyPathStyle);
            }

            // Clear button interaction
            if (!isEmpty)
            {
                var e       = Event.current;
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
                    SetKeyHash(propKeyHash, KeyId.Empty);
                    e.Use();
                    return;
                }
            }

            HandleKeyRowInput(rect, clearRect, propIsSpecific, propCollection, propKeyHash, keyId);
        }

        private void HandleKeyRowInput(Rect rect, Rect clearRect,
            SerializedProperty propIsSpecific,
            SerializedProperty propCollection,
            SerializedProperty propKeyHash,
            KeyId keyId)
        {
            var e = Event.current;
            if (!rect.Contains(e.mousePosition)) return;

            if (e.type == EventType.MouseDown && e.button == 0 && !clearRect.Contains(e.mousePosition))
            {
                var container = propIsSpecific.boolValue
                    ? propCollection.objectReferenceValue as IKeyCollectionContainer
                    : null;

                // Callback writes back through the SerializedProperty
                KeyWidgetWindowSelectorSearch.Open(newKeyId =>
                {
                    SetKeyHash(propKeyHash, newKeyId);
                }, container);

                e.Use();
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                ShowContextMenu(propKeyHash, keyId);
                e.Use();
            }
        }

        // ─── Write-back helper ────────────────────────────────────────────────────

        /// <summary>
        /// Writes a KeyId back through the serialized property and applies.
        /// Adjust the field access to match your KeyId's actual serialized layout.
        /// </summary>
        private static void SetKeyHash(SerializedProperty propKeyHash, KeyId keyId)
        {
            propKeyHash.intValue = keyId.Hash; // was .longValue
            propKeyHash.serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        // ─── Shared helpers ───────────────────────────────────────────────────────

        private void DrawThinBorder(Rect rect, Color color)
        {
            const float t = 1f;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - t, rect.width, t), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, t, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - t, rect.y, t, rect.height), color);
        }

        private static string GetFullPathWithKey(KeyId keyId)
        {
            var collection = KeyCollectionOwnerIndexer.Get(keyId);
            if (collection == null)
                return $"INVALID ({keyId.Hash})";

            var parts = new List<string>();
            foreach (var pathNode in collection.GetAllPaths())
                parts.Add(pathNode.Value);

            parts.Add(KeyItemIndexer.GetValue(keyId).Value);
            return string.Join(KeySelectorDrawerStyles.PathArrow, parts);
        }

        // ─── Context menu ─────────────────────────────────────────────────────────

        private void ShowContextMenu(SerializedProperty propKeyHash, KeyId keyId)
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
                    SetKeyHash(propKeyHash, KeyId.Empty));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(KeySelectorDrawerStyles.MenuItemNoKeySelected));
            }

            menu.ShowAsContext();
        }

        private static void ShowKeyInfo(KeyId keyId)
        {
            var keyValue = KeyItemIndexer.GetValue(keyId);
            EditorUtility.DisplayDialog(
                KeySelectorDrawerStyles.DialogTitleKeyInfo,
                string.Format(KeySelectorDrawerStyles.DialogMessageKeyInfo,
                    keyValue.Value,
                    keyId.Hash,
                    GetFullPathWithKey(keyId)),
                KeySelectorDrawerStyles.DialogButtonOK);
        }
    }
}