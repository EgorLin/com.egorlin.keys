using System.Collections.Generic;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Editor.Widgets.Dialogs;
using EgorLin.Keys.Editor.Widgets.Windows;
using EgorLin.Keys.Items.Data;
using EgorLin.Keys.Tags.Commands;
using EgorLin.Keys.Tags.Data;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Widgets.Paths
{
	public static class KeyWidgetPathSegment
	{
		private static readonly Color[] ColorsDepth = {
			new(0.3f, 0.8f, 0.4f),
			new(0.4f, 0.7f, 0.9f),
			new(0.9f, 0.7f, 0.3f),
			new(0.9f, 0.5f, 0.3f)
		};
		
		private const string LabelInvalidSegment = "⚠ INVALID";
		private const string TooltipInvalidSegment = "This tag no longer exists!";
		
		private static readonly GUILayoutOption[] LayoutOptionsInvalidSegment = {
			GUILayout.Height(22)
		};
		
		private static readonly Color ColorInvalidSegment = Color.red;
		private static readonly GUIStyle StyleTooltip = new(GUI.skin.button)
		{
			fontStyle = FontStyle.Bold,
			alignment = TextAnchor.MiddleCenter
		};

		private static readonly GUILayoutOption[] LayoutOptionsTooltip = {
			GUILayout.MinWidth(60),
			GUILayout.Height(22)
		};

		private const string LabelTooltip = "{0}\nDepth: {1}\nRight-click for options";

		private const string MenuItemReplace = "🔄 Replace";
		private const string MenuItemInfo = "⚠ Info";
		private const string MenuItemRemoveThisAndFollowing = "🗑 Remove";
		
        
		public static void Draw(List<KeyItem> pathNodes, int depthIndex)
		{
			var node = pathNodes[depthIndex];
			var tag = CommandKeyTagGetTag.Execute(node.TagId);
            
			if (tag.IsEmpty())
			{
				DrawInvalidSegment(pathNodes, depthIndex);
				
				return;
			}

			DrawSegment(pathNodes, depthIndex, tag);
		}

		private static void DrawSegment(List<KeyItem> pathNodes, int depthIndex, KeyTag tag)
		{
			var color = GetDepthColor(depthIndex);
			
			var tooltip = string.Format(LabelTooltip, tag.Value, depthIndex.ToString());

			if (KeyWidgetBase.DrawColoredButton(tag.Value, tooltip, color, StyleTooltip, 
				    LayoutOptionsTooltip))
			{
				ShowDropdown(pathNodes, depthIndex, tag);
			}
		}

		private static void DrawInvalidSegment(List<KeyItem> pathNodes, int depthIndex)
		{
			if (KeyWidgetBase.DrawColoredButton(LabelInvalidSegment, TooltipInvalidSegment, 
				    ColorInvalidSegment, LayoutOptionsInvalidSegment))
			{
				ShowDropdown(pathNodes, depthIndex, KeyTag.CreateEmpty());
			}
		}
		
		private static void ShowDropdown(List<KeyItem> pathNodes, int indexDepth, KeyTag tag)
		{
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent(MenuItemReplace), false, () =>
			{
				KeyWidgetWindowAddTag.Open(null, tagId =>
				{
					var node = pathNodes[indexDepth];

					node.SetTag(tagId);
					pathNodes[indexDepth] = node;
				});
			});

			menu.AddSeparator("");

			if (!tag.IsEmpty())
			{
				menu.AddItem(new GUIContent(MenuItemInfo), false, () =>
				{
					KeyWidgetDialogTag.Draw(tag);
				});
			}

			menu.AddSeparator("");

			menu.AddItem(new GUIContent(MenuItemRemoveThisAndFollowing), false, () =>
			{
				RemovePathSegments(pathNodes, indexDepth);
			});

			menu.ShowAsContext();
		}
		
		private static void RemovePathSegments(List<KeyItem> pathNodes, int indexDepth)
		{
			var countToRemove = pathNodes.Count - indexDepth;
            
			pathNodes.RemoveRange(indexDepth, countToRemove);
		}
		
		private static Color GetDepthColor(int depth)
		{
			var color = ColorsDepth[Mathf.Min(depth, ColorsDepth.Length - 1)];
			
			return color;
		}
	}
}
