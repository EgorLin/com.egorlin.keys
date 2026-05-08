using EgorLin.Keys.Editor.Widgets.Base;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemCopyButton
	{
		public const string TextDebug = "Copied: {0}";
		private const string TextLabel = "📋";
		private const string TextTooltip = "Copy constant name";

		public static bool Draw()
		{
			return KeyWidgetBase.DrawButtonWidthSmall(TextLabel, TextTooltip);
		}
	}
}
