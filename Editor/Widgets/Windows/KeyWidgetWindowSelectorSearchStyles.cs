using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
	public static class KeyWidgetWindowSelectorSearchStyles
	{
		// Window configuration
		public const int WindowWidth = 500;
		public const int WindowHeight = 600;
		public const int WindowOffsetX = 250;
		public const int WindowOffsetY = 300;
		public const string WindowTitle = "🔑 Select Key";
		
		// Layout constants
		public const float EntryHeight = 50f;
		public const float EntryLeftBorderWidth = 3f;
		public const float EntryIconSize = 20f;
		public const float EntryIconPadding = 10f;
		public const float EntryContentRightReserve = 150f;
		public const float EntryHashWidth = 90f;
		public const float IconTextGap = 5f;
		public const float EntryHashOffset = 100f;
		public const float EntryPathOffsetY = 2f;
		public const float TitleHeight = 30f;
		public const float SearchIconWidth = 20f;
		public const float SearchClearButtonWidth = 20f;
		
		// Spacing
		public const int SpaceAfterHeader = 3;
		public const int SpaceAfterSearch = 3;
		public const int SpaceAfterRecentSection = 10;
		public const int SpaceAfterSearchBar = 5;
		public const int SpaceBetweenEntries = 2;
		public const int SpaceBetweenGroups = 8;
		
		// Colors
		public static readonly Color ColorBackgroundRecent = new(0.3f, 0.5f, 0.7f, 0.2f);
		public static readonly Color ColorBackgroundNormal = new(0.2f, 0.2f, 0.2f, 0.2f);
		public static readonly Color ColorUsageHigh = new(0.4f, 0.8f, 0.4f);
		public static readonly Color ColorUsageMedium = new(0.8f, 0.8f, 0.4f);
		public static readonly Color ColorUsageLow = new(0.5f, 0.5f, 0.5f);
		public static readonly Color ColorGroupHeader = new(0.6f, 0.8f, 1f);
		public static readonly Color ColorPathText = new(0.6f, 0.6f, 0.6f);
		public static readonly Color ColorHashText = new(0.5f, 0.5f, 0.5f);
		public const float AlphaHoverIncrease = 0.2f;

		// Text constants
		public const string IconSearch = "🔍";
		public const string IconRecent = "🕐";
		public const string IconKey = "🔑";
		public const string IconFolder = "📁";
		public const string LabelSearchClear = "×";
		public const string LabelRecentKeys = "🕐 Recently Used";
		public const string LabelAllKeys = "All Keys ({0})";
		public const string LabelSearchResults = "Search Results ({0})";
		public const string LabelHashFormat = "#{0}";
		public const string LabelGroupHeaderFormat = "📁 {0}";
		
		// Messages
		public const string MessageNoKeysFound = "No keys found in any KeyCollection ScriptableObjects";
		public const string MessageNoMatchFormat = "No keys match '{0}'";
		
		// Control names
		public const string ControlNameSearchField = "SearchField";
		
		// Search style name
		public const string StyleNameToolbarSearch = "ToolbarSearchTextField";
		
		// Constants
		public const int RecentKeysDisplayCount = 3;
		public const int RecentKeysMaxCount = 10;
		public const int UsageCountHighThreshold = 5;
		public const int UsageCountMediumThreshold = 2;
		
		// Asset search
		public const string AssetSearchFilter = "t:KeyCollectionAsset";
		
		// Style factories
		public static GUIStyle CreateTitleStyle()
		{
			return new GUIStyle(EditorStyles.boldLabel) 
			{ 
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter
			};
		}
		
		public static GUIStyle CreateGroupHeaderStyle()
		{
			return new GUIStyle(EditorStyles.boldLabel)
			{
				fontSize = 11,
				normal = { textColor = ColorGroupHeader }
			};
		}
		
		public static GUIStyle CreateIconStyle(int fontSize = 18)
		{
			return new GUIStyle(EditorStyles.label) { fontSize = fontSize };
		}
		
		public static GUIStyle CreateEntryNameStyle()
		{
			return new GUIStyle(EditorStyles.boldLabel) { fontSize = 13 };
		}
		
		public static GUIStyle CreatePathStyle()
		{
			return new GUIStyle(EditorStyles.miniLabel)
			{
				fontSize = 10,
				normal = { textColor = ColorPathText }
			};
		}
		
		public static GUIStyle CreateHashStyle()
		{
			return new GUIStyle(EditorStyles.miniLabel)
			{
				alignment = TextAnchor.MiddleRight,
				fontSize = 9,
				normal = { textColor = ColorHashText }
			};
		}
		
		public static Color GetBackgroundColor(bool isRecent, bool isHovered)
		{
			var color = isRecent ? ColorBackgroundRecent : ColorBackgroundNormal;
			
			if (isHovered)
			{
				color.a += AlphaHoverIncrease;
			}
			
			return color;
		}
		
		public static string GetResultsLabel(string search, int count)
		{
			var template = string.IsNullOrEmpty(search) ? LabelAllKeys : LabelSearchResults;
			return string.Format(template, count);
		}
		
		public static string GetNoResultsMessage(string search)
		{
			return string.IsNullOrEmpty(search) 
				? MessageNoKeysFound 
				: string.Format(MessageNoMatchFormat, search);
		}
		
		public static void DrawColoredLabel(string text, Color color, GUIStyle style)
		{
			var prevColor = GUI.color;
			GUI.color = color;
			GUI.Label(new Rect(), text, style);
			GUI.color = prevColor;
		}
	}
}
