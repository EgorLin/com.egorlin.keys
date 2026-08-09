using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Windows
{
	public static class KeyWidgetWindowAddStyles
	{
		// Window configuration
		public const int WindowWidth = 400;
		public const int WindowHeight = 500;
		public const string WindowTitle = "Add Key";
		
		// Colors
		public static readonly Color ColorCreateNewButton = new(0.4f, 0.8f, 0.4f);
		public static readonly Color ColorExistingTagButton = new(0.4f, 0.7f, 1f);
		public static readonly Color ColorLockedTagButton = new(0.8f, 0.4f, 0.4f);
		public static readonly Color ColorUsageHigh = Color.white;
		public static readonly Color ColorUsageLow = Color.gray;
		
		// Spacing
		public const int SpaceAfterSearch = 5;
		public const int SpaceAfterCreateNew = 5;
		public const int SpaceAfterRecentTags = 10;
		
		// Layout options
		public static readonly GUILayoutOption LayoutOptionCreateButtonHeight = GUILayout.Height(35);
		public static readonly GUILayoutOption LayoutOptionScrollMaxHeight = GUILayout.MaxHeight(300);
		public static readonly GUILayoutOption LayoutOptionUsageCountWidth = GUILayout.Width(40);
		
		// Labels and text
		public const string LabelHeader = "Select or Create Tag";
		public const string LabelRecentTags = "Recently Used";
		public const string LabelAvailableTags = "Available Tags";
		public const string LabelUnavailableTags = "Unavailable Tags";
		public const string LabelResultsCount = "{0} ({1})";
		
		// Button labels
		public const string LabelCreateNewTag = "+ Create New Tag: \"{0}\"";
		public const string LabelUseExistingTag = "✓ Use Existing Tag: \"{0}\"";
		public const string LabelLockedTag = "❌ Locked Tag: \"{0}\"";
		public const string IconRecent = "🕐";
		public const string IconTag = "🏷";
		public const string LabelTagButton = "{0} {1}";
		
		// Messages
		public const string MessageNoResults = "No existing tags match '{0}'\nClick the button above to create it!";
		
		// Control names
		public const string ControlNameSearchField = "SearchField";
		
		// Toolbar style name
		public const string StyleNameToolbarSearch = "ToolbarSearchTextField";
		
		// Constants
		public const int RecentTagsDisplayCount = 5;
		
		// Styles
		public static GUIStyle GetHeaderStyle()
		{
			return new GUIStyle(EditorStyles.boldLabel) { fontSize = 14 };
		}
		
		public static GUIStyle GetTagButtonStyle()
		{
			return new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
		}
		
		public static Color GetTagColor(bool isLocked)
		{
			if (isLocked)
			{
				return ColorUsageLow;
			}

			return ColorUsageHigh;
		}
		
		public static string GetResultsLabel(bool areTagsLocked, int count)
		{
			var label = areTagsLocked ? LabelUnavailableTags : LabelAvailableTags;
			return string.Format(LabelResultsCount, label, count);
		}
		
		public static bool DrawColoredButton(string label, Color color, params GUILayoutOption[] options)
		{
			var prevColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			
			var clicked = GUILayout.Button(label, options);
			
			GUI.backgroundColor = prevColor;
			
			return clicked;
		}
		
		public static bool DrawTagButton(string icon, string tagValue, bool isLocked, GUIStyle style)
		{
			var prevColor = GUI.color;
			GUI.color = GetTagColor(isLocked);
			
			var label = string.Format(LabelTagButton, icon, tagValue);
			var clicked = GUILayout.Button(label, style);
			
			GUI.color = prevColor;
			
			return !isLocked && clicked;
		}
	}
}
