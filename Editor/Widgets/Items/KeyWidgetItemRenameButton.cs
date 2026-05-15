using EgorLin.Keys.Editor.Widgets.Base;

namespace EgorLin.Keys.Editor.Widgets.Items
{
	public static class KeyWidgetItemRenameButton
	{
		private const string TextLabel = "✏";
		private const string TextTooltip = "Rename tag name";

		public static bool Draw()
		{
			return KeyWidgetBase.DrawButtonWidthSmall(TextLabel, TextTooltip);
		}
	}
}
