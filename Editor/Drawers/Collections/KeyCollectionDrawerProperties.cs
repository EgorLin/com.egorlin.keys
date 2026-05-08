using EgorLin.Collections.Unsafe;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace EgorLin.Keys.Editor.Drawers.Collections
{
    [InitializeOnLoad]
	public class KeyCollectionDrawerProperties
	{
		private static readonly IntHashMap<PropertyTree> PropertyTrees = new();
		
		static KeyCollectionDrawerProperties()
		{
			Selection.selectionChanged += Clear;
			AssemblyReloadEvents.beforeAssemblyReload += Clear;
		}
		
		public static bool TryGet(int id, out PropertyTree tree)
		{
			return PropertyTrees.TryGetValue(id, out tree);
		}
		
		public static void Set(int id, PropertyTree tree)
		{
			PropertyTrees.Set(id, tree, out _);
		}

		public static void Remove(int id)
		{
			if (PropertyTrees.Remove(id, out var tree))
			{
				tree!.Dispose();
			}
		}

		private static void Clear()
		{
            foreach (int id in PropertyTrees)
            {
                var valueByKey = PropertyTrees.GetValueByKey(id);
                valueByKey?.Dispose();
            }
            
            PropertyTrees.Clear();
		}
	}
}
