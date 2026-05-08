using UnityEditor;

namespace EgorLin.Keys.Editor.Widgets.Base
{
	public class KeyWidgetInfoBox
	{
		private const string Message = "Don't copy this Scriptable Object or Component. Always create new one. Also add keys only for SO or prefabs. Do not add to scenes";
		
		public static void Draw()
		{
			EditorGUILayout.HelpBox(Message, MessageType.Info);
			KeyWidgetBase.DrawSpaceSmall();
		}
	}
}
