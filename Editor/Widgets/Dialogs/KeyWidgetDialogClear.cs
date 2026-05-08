using UnityEditor;

namespace EgorLin.Keys.Editor.Widgets.Dialogs
{
	public static class KeyWidgetDialogClear
	{
		private const string TextTitle = "Clear All Keys";
		private const string TextMessage = "Are you sure you want to remove all {0} keys?";
		private const string TextYes = "Yes";
		private const string TextCancel = "Cancel";
		
		public static bool Draw(int keysCount)
		{
			var message = string.Format(TextMessage, keysCount);
			
            return EditorUtility.DisplayDialog(TextTitle, message, TextYes, TextCancel);
		}
	}
}
