#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Backend.Database
{
	public static class KeyTagDatabaseProvider
	{
		private const string SaveAssetPath = "Assets/Resources/KeyTagDatabase.asset";
		private const string LoadResourcePath = "KeyTagDatabase";
		
		private static KeyTagDatabase _assetDataBase;
		
		public static KeyTagDatabase Get()
		{
			if (_assetDataBase == null)
			{
				_assetDataBase = Load();
			}

			return _assetDataBase;
		}

		private static KeyTagDatabase Load()
		{
			var assetDataBase = Resources.Load<KeyTagDatabase>(LoadResourcePath);

			if (assetDataBase == null)
			{
				assetDataBase = ScriptableObject.CreateInstance<KeyTagDatabase>();

				if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				{
					AssetDatabase.CreateFolder("Assets", "Resources");
				}
				
				AssetDatabase.CreateAsset(assetDataBase, SaveAssetPath);
			}
            
			return assetDataBase;
		}
	}
}
#endif
