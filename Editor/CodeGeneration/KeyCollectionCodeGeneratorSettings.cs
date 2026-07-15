using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Editor.CodeGeneration
{
	[CreateAssetMenu(fileName = "KeyCollectionCodeGeneratorSettings", menuName = "Keys/Code Generator Settings")]
	public class KeyCollectionCodeGeneratorSettings : ScriptableObject
	{
		[Tooltip("Folder where generated code files will be saved")]
		public string OutputFolder = "Assets/App/Scripts/Data/Keys";
		
		[Tooltip("Name of the partial class that will contain all keys")]
		public string ClassName = "Keys";
		
		[Tooltip("Namespace for generated classes (leave empty for no namespace)")]
		public string Namespace = "App.Scripts.Data.Keys";
		
		[Tooltip("Namespace for KeyId (leave empty for no namespace)")]
		public string NamespaceKeyId = "App.Scripts.Modules.Keys.Ids";
		
		private static KeyCollectionCodeGeneratorSettings instance;
		
		public static KeyCollectionCodeGeneratorSettings GetOrCreate()
		{
			if (instance != null)
			{
				return instance;
			}
			
			var guids = AssetDatabase.FindAssets($"t:{nameof(KeyCollectionCodeGeneratorSettings)}");
			
			if (guids.Length > 0)
			{
				var path = AssetDatabase.GUIDToAssetPath(guids[0]);
				instance = AssetDatabase.LoadAssetAtPath<KeyCollectionCodeGeneratorSettings>(path);
				return instance;
			}
			
			// Create default settings
			instance = CreateInstance<KeyCollectionCodeGeneratorSettings>();
			
			var folderPath = "Assets/Settings";
			if (!AssetDatabase.IsValidFolder(folderPath))
			{
				AssetDatabase.CreateFolder("Assets", "Settings");
			}
			
			var assetPath = $"{folderPath}/{nameof(KeyCollectionCodeGeneratorSettings)}.asset";
			AssetDatabase.CreateAsset(instance, assetPath);
			AssetDatabase.SaveAssets();
			
			return instance;
		}
	}
}