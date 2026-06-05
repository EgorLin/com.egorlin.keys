using System;
using System.Reflection;

namespace EgorLin.Keys.Editor.Drawers.Utils
{
    public static class ReflectionUtils
    {
        private const BindingFlags Flags =
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public;
 
        public static FieldInfo GetFieldInfo(Type type, string propertyPath)
        {
            if (propertyPath.Contains('.') || propertyPath.Contains('['))
            {
                return null;
            }
 
            var t = type;
 
            while (t != null && t != typeof(object))
            {
                var fi = t.GetField(propertyPath, Flags);
 
                if (fi != null)
                {
                    return fi;
                }
 
                t = t.BaseType;
            }
 
            return null;
        }
    }
}