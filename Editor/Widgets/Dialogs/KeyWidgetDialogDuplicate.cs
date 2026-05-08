using UnityEditor;

namespace EgorLin.Keys.Editor.Widgets.Dialogs
{
	public static class KeyWidgetDialogDuplicate
	{
        private const string TextTitle = "Duplicate Key";
        private const string TextMessage = "A key with this ID already exists in this collection.";
        private const string TextYes = "Yes";

        public static bool Draw()
        {
	        return EditorUtility.DisplayDialog(TextTitle, TextMessage, TextYes);
        }
	}
}
