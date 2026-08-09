using EgorLin.Keys.Editor.Widgets.Base;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemCopyButton
	{
		public const string TextDebug = "Copied: {0}";
		public const string TextTooltipTag = "Copy tag name";
		public const string TextTooltipId = "Copy id value";
		
		private const string TextLabel = "📋";

		public static bool Draw(string tooltip)
		{
			return KeyWidgetBase.DrawButtonWidthSmall(TextLabel, tooltip);
		}
	}
}
