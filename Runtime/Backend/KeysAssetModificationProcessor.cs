#if UNITY_EDITOR
using System.Collections.Generic;
using EgorLin.Keys.Owners;
using UnityEditor;

namespace EgorLin.Keys.Backend
{
	public class KeysAssetModificationProcessor : AssetModificationProcessor
	{
		private static readonly HashSet<string> PendingPaths = new();
		private static bool _isScheduled;
			
		private static void OnWillCreateAsset(string assetName)
		{
			if (!assetName.EndsWith(".meta"))
			{
				return;
			}

			var path = assetName.Replace(".meta", "");
			PendingPaths.Add(path);

			ScheduleProcess();
		}

		private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
		{
			PendingPaths.Add(assetPath);
			
			ScheduleProcess();

			return AssetDeleteResult.DidNotDelete;
		}

		private static void ScheduleProcess()
		{
			if (_isScheduled)
			{
				return;
			}

			_isScheduled = true;

			ProcessPending();
		}

		private static void ProcessPending()
		{
			_isScheduled = false;

			var needsRebuild = false;

			foreach (var path in PendingPaths)
			{
				if (IsCollectionContainer(path))
				{
					needsRebuild = true;
					break;
				}
			}

			PendingPaths.Clear();

			if (needsRebuild)
			{
				KeysBackend.Rebuild();
			}
		}

		private static bool IsCollectionContainer(string assetPath)
		{
			var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
			
			foreach (var asset in assets)
			{
				if (asset is IKeyCollectionContainer)
				{
					return true;
				}
			}

			return false;
		}
	}
}
#endif
