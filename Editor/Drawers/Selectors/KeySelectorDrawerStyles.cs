using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Selectors
{
    public static class KeySelectorDrawerStyles
    {
        // Layout
        public const float SourceRowHeight   = 22f;
        public const float KeyRowHeight      = 26f;
        public const float BodyPadding       = 8f;
        public const float RowGap            = 6f;
        public const float ToggleWidth       = 28f;
        public const float ToggleHeight      = 14f;
        public const float IconColumnWidth   = 28f;
        public const float ClearButtonWidth  = 22f;
        public const float AccentBorderWidth = 3f;

        // Colors
        public static readonly Color ColorAccentBorder       = new(0.27f, 0.53f, 0.84f, 1f);
        public static readonly Color ColorBodyBackground     = new(0.15f, 0.15f, 0.15f, 0.25f);
        public static readonly Color ColorGroupBorder        = new(0.35f, 0.35f, 0.35f, 0.5f);
        public static readonly Color ColorRowBackground      = new(0.22f, 0.22f, 0.22f, 0.5f);
        public static readonly Color ColorRowBorder          = new(0.3f,  0.3f,  0.3f,  0.4f);
        public static readonly Color ColorIconBgSet          = new(0.18f, 0.38f, 0.6f,  0.5f);
        public static readonly Color ColorIconBgEmpty        = new(0.55f, 0.18f, 0.18f, 0.5f);
        public static readonly Color ColorIconFgSet          = new(0.5f,  0.75f, 1f,    1f);
        public static readonly Color ColorIconFgEmpty        = new(1f,    0.45f, 0.45f, 1f);
        public static readonly Color ColorToggleOn           = new(0.25f, 0.5f,  0.85f, 0.8f);
        public static readonly Color ColorToggleOff          = new(0.3f,  0.3f,  0.3f,  0.6f);
        public static readonly Color ColorToggleThumb        = new(0.9f,  0.9f,  0.9f,  1f);
        public static readonly Color ColorTextPrimary        = new(0.88f, 0.88f, 0.88f, 1f);
        public static readonly Color ColorTextSecondary      = new(0.6f,  0.6f,  0.6f,  1f);
        public static readonly Color ColorTextHint           = new(0.45f, 0.45f, 0.45f, 1f);
        public static readonly Color ColorClearHover         = new(1f,    0.38f, 0.38f, 1f);

        // Text
        public const string LabelNoKeySelected = "No key selected — click to assign";
        public const string LabelAnyCollection = "Any collection";
        public const string PathArrow          = " / ";
        public const string IconKey            = "🔑";
        public const string IconWarning        = "⚠";

        // Context menu
        public const string MenuItemCopyHash       = "Copy Hash";
        public const string MenuItemCopyPath       = "Copy Path";
        public const string MenuItemShowInProject  = "Show in Project";
        public const string MenuItemInfo           = "Show Info";
        public const string MenuItemClear          = "Clear";
        public const string MenuItemNoKeySelected  = "No key selected";

        // Dialog
        public const string DialogTitleKeyInfo      = "Key Information";
        public const string DialogMessageKeyInfo    = "Tag: {0}\nHash: {1}\nFull Path: {2}";
        public const string DialogButtonOK          = "OK";
        public const string LogCopiedHash           = "Copied hash: {0}";
        public const string LogCopiedPath           = "Copied path: {0}";
        
        public const float SourceLabelWidth = 140f;

        // Style factories
        public static GUIStyle CreateFieldLabelStyle() => new(EditorStyles.label)
        {
            fontSize  = 11,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            padding   = new RectOffset(0, 4, 0, 0),
            normal    = { textColor = ColorTextPrimary }
        };

        public static GUIStyle CreateKeyPathStyle() => new(EditorStyles.label)
        {
            fontSize    = 11,
            alignment   = TextAnchor.MiddleLeft,
            padding     = new RectOffset(6, 4, 0, 0),
            normal      = { textColor = ColorTextPrimary }
        };

        public static GUIStyle CreateKeyPathEmptyStyle() => new(EditorStyles.label)
        {
            fontSize    = 11,
            fontStyle   = FontStyle.Italic,
            alignment   = TextAnchor.MiddleLeft,
            padding     = new RectOffset(6, 4, 0, 0),
            normal      = { textColor = ColorTextHint }
        };

        public static GUIStyle CreateIconStyle() => new(EditorStyles.label)
        {
            fontSize    = 12,
            alignment   = TextAnchor.MiddleCenter,
            padding     = new RectOffset(0, 0, 0, 0)
        };

        public static GUIStyle CreateCollectionNameStyle() => new(EditorStyles.label)
        {
            fontSize    = 11,
            alignment   = TextAnchor.MiddleLeft,
            padding     = new RectOffset(4, 4, 0, 0),
            normal      = { textColor = ColorTextSecondary }
        };
    }
}
