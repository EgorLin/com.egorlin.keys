using System.Collections.Generic;
using EgorLin.Keys.Collections.Data;
using EgorLin.Keys.Editor.Widgets.Base;
using EgorLin.Keys.Items.Data;
using UnityEditor;

namespace EgorLin.Keys.Editor.Widgets.Paths
{
	public static class KeyWidgetPathRoot
	{
		public static void Draw(KeyCollectionBase collection, ModelKeyWidgetPathRoot model)
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			UpdatePath(collection.Paths, model);
            
			KeyWidgetPathHeader.Draw(model.TextFormatedPath, collection.Paths.Count);
            
			KeyWidgetBase.DrawSpaceSmall();
            
			KeyWidgetPathList.DrawPath(collection);
            
			EditorGUILayout.EndVertical();
		}
		
		private static void UpdatePath(List<KeyItem> pathItems, ModelKeyWidgetPathRoot model)
		{
			if (model.IsDirty)
			{
				model.SetTextFormatedPath(KeyWidgetPathHeader.GetLabelPath(pathItems));
				model.SetIsDirty(false);
			}
		}
	}

	public class ModelKeyWidgetPathRoot
	{
		public bool IsDirty = true;
        public string TextFormatedPath;

        public void SetIsDirty(bool value)
        {
	        IsDirty = value;
        }
        
        public void SetTextFormatedPath(string text)
        {
	        TextFormatedPath = text;
        }
	}
}
