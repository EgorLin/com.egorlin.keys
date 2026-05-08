#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace EgorLin.Keys.Owners
{
	public class KeyPropertyOwnerResolver
	{
        public static Object Resolve(InspectorProperty property)
        {
            var parent = property.Parent;
            
            while (parent != null)
            {
                if (parent.ValueEntry?.WeakSmartValue is Object obj && obj != null)
                {
                    return obj;
                }
                
                parent = parent.Parent;
            }
            
            return null;
        }
	}
}
#endif
