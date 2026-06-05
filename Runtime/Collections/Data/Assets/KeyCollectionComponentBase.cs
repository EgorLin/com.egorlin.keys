using System.Collections.Generic;
using EgorLin.Keys.Backend;
using EgorLin.Keys.Owners;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace EgorLin.Keys.Collections.Data.Assets
{
	[ExecuteInEditMode]
	public abstract class KeyCollectionComponentBase : MonoBehaviour, IKeyCollectionContainer
	{
		public abstract IEnumerable<IKeyCollectionOwner> GetCollections();
		
#if UNITY_EDITOR
		private void OnEnable()
		{
			foreach (var collection in GetCollections())
			{
				collection.ValidateKeys();

				if (!collection.HasOwner())
				{
					collection.SetOwner(this);
				}
			}
		}
		
		private void OnDestroy()
		{
			if (IsBeingDeleted())
			{
				KeysBackend.Rebuild();
			}
		}

		private bool IsBeingDeleted()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return false;
			}

			if (EditorApplication.isCompiling)
			{
				return false;
			}
			
			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			
			if (prefabStage == null)
			{
				return false;
			}

			return true;
		}
#endif
	}
}
