using System;
using System.Collections.Generic;
using EgorLin.Collections.Lists;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Tags.Data;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EgorLin.Keys.Backend.Database
{
	public class KeyTagDatabase : SerializedScriptableObject
	{
		[SerializeField] [ReadOnly] private List<KeyTag> tags = new();

		public KeyTag CreateTag(string value)
		{
			var tag = KeyTag.Create(value);

			if (tags.Contains(tag))
			{
				Debug.LogError($"Tag {value} already exists");
			}
			else
			{
				tags.Add(tag);
			}

#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
			
			return tag;
		}

		public KeyTag GetTag(KeyId id)
		{
			var tagToFind = KeyTag.CreateWithEmptyValue(id);
			var index = tags.IndexOf(tagToFind);

			if (index >= 0)
			{
				return tags[index];
			}
			
			Debug.LogError($"Tag with id {id.ToString()} not found");
			
			return tagToFind;
		}

		public ReadOnlySpan<KeyTag> GetTags()
		{
			return tags.AsReadOnlySpan();
		}

		public void Remove(KeyTag tag)
		{
			tags.Remove(tag);
			
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}
}