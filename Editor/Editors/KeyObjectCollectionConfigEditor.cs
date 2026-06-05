using EgorLin.Keys.Collections.Data.Assets;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.Editors
{
    [CustomEditor(typeof(KeyCollectionConfigBase), editorForChildClasses: true)]
    public class KeyObjectCollectionConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var prop = serializedObject.GetIterator();
            prop.NextVisible(true);

            while (prop.NextVisible(false))
            {
                EditorGUILayout.PropertyField(prop, false);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}