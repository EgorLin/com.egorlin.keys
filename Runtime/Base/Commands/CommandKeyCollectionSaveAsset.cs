#if UNITY_EDITOR
using EgorLin.Keys.Owners;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EgorLin.Keys.Base.Commands
{
	public static class CommandKeyCollectionSaveAsset
	{
		public static void Execute(InspectorProperty property)
		{
			var owner = KeyPropertyOwnerResolver.Resolve(property);
			
			if (owner == null)
			{
				return;
			}
			
			Execute(owner);
		}

		private static void Execute(Object obj)
		{
			EditorUtility.SetDirty(obj);
			
			if (obj is MonoBehaviour mb)
			{
				var go = mb.gameObject;

				var stage = PrefabStageUtility.GetCurrentPrefabStage();
				if (stage != null)
				{
					PrefabUtility.SaveAsPrefabAsset(stage.prefabContentsRoot, stage.assetPath);
				}
				else
				{
					if (PrefabUtility.IsPartOfPrefabInstance(go))
					{
						var root = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
						PrefabUtility.ApplyPrefabInstance(root, InteractionMode.AutomatedAction);
					}
					else if (PrefabUtility.IsPartOfPrefabAsset(go))
					{
						var root = PrefabUtility.GetOutermostPrefabInstanceRoot(go) ?? go;
						PrefabUtility.SavePrefabAsset(root);
					}
				}

				if (go.scene.IsValid())
				{
					EditorSceneManager.MarkSceneDirty(go.scene);
				}
			}
			
			AssetDatabase.SaveAssets();
		}
	}
}
#endif