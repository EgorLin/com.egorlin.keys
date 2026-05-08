using EgorLin.Keys.Tags.Data;
using UnityEditor;

namespace EgorLin.Keys.Editor.Widgets.Dialogs
{
	public static class KeyWidgetDialogTag
	{
		private const string DialogTitleTagInfo = "Tag Info";
		private const string DialogMessageTagInfo = "Tag: {0}\nHash: {1}";
		private const string DialogButtonOK = "OK";
		
		public static void Draw(KeyTag tag)
		{
			var message = string.Format(DialogMessageTagInfo, tag.Value, tag.Id.Hash);
			
			EditorUtility.DisplayDialog(DialogTitleTagInfo, message, DialogButtonOK);
		}
	}
}
