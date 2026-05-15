using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Drawers.Selectors
{
	public static class KeySelectorDrawerStyles
	{
		// Layout constants
		public const float CompactHeight = 24f;
		public const float IconSize = 18f;
		public const float IconPadding = 6f;
		
		// Colors
		public static readonly Color ColorBackgroundEmpty = new(0.4f, 0.25f, 0.25f, 0.5f);
		public static readonly Color ColorBackgroundSet = new(0.2f, 0.3f, 0.4f, 0.3f);
		public static readonly Color ColorTextEmpty = new(0.6f, 0.6f, 0.6f);
		public static readonly Color ColorTextSet = new(0.9f, 0.9f, 0.9f);
		
		// Text constants
		public const string IconEmpty = "⚠️";
		public const string IconSet = "🔑";
		public const string LabelNoKeySelected = "No Key Selected - Click to select";
		public const string LabelPathArrow = "/";
		public const string LabelNoPath = "No path";
		
		// Context menu items
		public const string MenuItemCopyHash = "📋 Copy Hash";
		public const string MenuItemCopyPath = "📋 Copy Path";
		public const string MenuItemShowInProject = "🔍 Show in Project";
		public const string MenuItemInfo = "ℹ️ Show Info";
		public const string MenuItemClear = "🗑 Clear";
		public const string MenuItemNoKeySelected = "No key selected";
		
		// Dialog
		public const string DialogTitleKeyInfo = "Key Information";
		public const string DialogMessageKeyInfo = "Tag: {0}\nHash: {1}\nFull Path: {2}";
		public const string DialogButtonOK = "OK";
		
		// Log messages
		public const string LogCopiedHash = "Copied hash: {0}";
		public const string LogCopiedPath = "Copied path: {0}";
		
		// Style factories
		public static GUIStyle CreateButtonStyle()
		{
			return new GUIStyle(EditorStyles.label)
			{
				alignment = TextAnchor.MiddleLeft,
				fontSize = 11,
				fontStyle = FontStyle.Normal,
				padding = new RectOffset(5, 5, 3, 3),
				normal = { textColor = new Color(0.8f, 0.8f, 0.8f) }
			};
		}
		
		public static GUIStyle CreatePathStyle()
		{
			return new GUIStyle(EditorStyles.label)
			{
				fontSize = 12,
				fontStyle = FontStyle.Normal,
				alignment = TextAnchor.MiddleLeft,
				padding = new RectOffset(5, 5, 3, 3),
				normal = { textColor = new Color(0.8f, 0.8f, 0.8f) }
			};
		}
		
		public static GUIStyle CreateIconStyle()
		{
			return new GUIStyle(EditorStyles.label) 
			{ 
				fontSize = 12,
				alignment = TextAnchor.MiddleCenter 
			};
		}
		
		// Helper methods
		public static void ConfigureButtonStyleForEmpty(GUIStyle style)
		{
			style.normal.textColor = ColorTextEmpty;
			style.fontStyle = FontStyle.Italic;
		}
		
		public static void ConfigureButtonStyleForSet(GUIStyle style)
		{
			style.normal.textColor = ColorTextSet;
			style.fontStyle = FontStyle.Normal;
		}
	}
}
