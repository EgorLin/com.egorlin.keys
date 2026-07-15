#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Owners
{
	public class KeyPropertyOwnerResolver
	{
		public static Object Resolve(SerializedProperty property)
        {
            return property?.serializedObject?.targetObject;
        }
	}
}
#endif
